using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class Player : GameEntity
{
    public static float DEFAULT_SPEED = 7.0f;
    public static float DEFAULT_LIFE = 3;
    public static int MAX_SCORE = 999999;
    public static int MAX_SKILL = 8;
    public static int MAX_BULLET_TYPES = 1;
    public static int REWARD_LIFE_CONDITION = 10000;
    public static float MAX_SKILL_CD_TIME = 2.0f;

    public GameObject[] PlayerBullets = new GameObject[MAX_BULLET_TYPES];

    private int mScore;
    private int mBulletLevel;
    private SkillManager mSkillMgr;
    private int mNextRewardLife;
    private float mSkillCDTimer;

    private PostProcessProfile pp;

    public int Score
    {
        set
        {
            mScore = Mathf.Clamp(value, 0, MAX_SCORE);
            RewardLife();
        }
        get { return mScore; }
    }

    public int SkillCount
    {
        get { return mSkillMgr.GetSkillCount(); }
    }

    public BaseSkill SkillData
    {
        get { return mSkillMgr.GetSkillData(); }
    }

    private void InitSelf()
    {
        base.mSpeed = DEFAULT_SPEED;
        base.mLife = DEFAULT_LIFE;
        this.transform.position = new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(-4.0f, 4.0f));
        this.mScore = 0;
        this.mSkillMgr = gameObject.GetComponent<SkillManager>();
        this.mBulletLevel = 0;
        this.mNextRewardLife = REWARD_LIFE_CONDITION;
        this.mSkillCDTimer = 0.0f;

        this.pp = GameWorld.Instance.postProcessProfile;
    }

    protected virtual void Start()
    {
        InitSelf();
    }

    protected virtual void Update()
    {
        //死亡
        if (mLife <= 0)
        {
            //记录“上次得分”
            PlayerPrefs.SetInt("_Last_Score_", mScore);
            //记录“最高分”
            if (mScore > PlayerPrefs.GetInt("_High_Score_"))
            {
                PlayerPrefs.SetInt("_High_Score_", mScore);
            }
            AudioManager.Instance.StopMusic();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Player.Life == 0. GAME OVER !");
        }

        UpdateVelocityByKeyboard();
        Shooting();
        Skilling();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected void UpdateVelocityByKeyboard()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        mMoveVelocity = input.normalized * mSpeed;
    }

    //普通攻击
    private void Shooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(PlayerBullets[mBulletLevel], this.transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySound((int) AudioConstant.SHOOT01);
        }
    }

    private float musicVolumeBeforeSkill = -1f;
    
    //施放技能
    private void Skilling()
    {
        //若处于技能CD中
        if (mSkillCDTimer > 0.0f)
        {
            mSkillCDTimer -= Time.deltaTime;

            //控制屏幕特效
            pp.GetSetting<ChromaticAberration>().intensity.value = mSkillCDTimer / MAX_SKILL_CD_TIME;
            pp.GetSetting<Vignette>().smoothness.value = mSkillCDTimer / MAX_SKILL_CD_TIME;

            //控制声音
            if (musicVolumeBeforeSkill == -1f) musicVolumeBeforeSkill = AudioManager.Instance.MusicVolume;
            AudioManager.Instance.ChangeMusicVolume(Mathf.Clamp01(0.02f + musicVolumeBeforeSkill - mSkillCDTimer / MAX_SKILL_CD_TIME));
            return;
        }
        //若技能可用
        else if (mSkillCDTimer <= 0.0f && Input.GetMouseButtonDown(1))
        {
            if (mSkillMgr.UseSkill())
            {
                mSkillCDTimer = MAX_SKILL_CD_TIME;

                if (musicVolumeBeforeSkill != -1f) AudioManager.Instance.ChangeMusicVolume(musicVolumeBeforeSkill);
                musicVolumeBeforeSkill = -1f;
            }
            else
            {
                //TODO 播放空声音
            }
        }
    }

    //奖励生命
    private void RewardLife()
    {
        if (mScore <= 0) return;

        if (mScore >= mNextRewardLife)
        {
            mScore += (mScore / mNextRewardLife);

            mNextRewardLife += REWARD_LIFE_CONDITION;
            AudioManager.Instance.PlaySound((int) AudioConstant.LIFE_GET);
        }
    }

    public SkillManager GetSkillMgr()
    {
        return mSkillMgr;
    }
}