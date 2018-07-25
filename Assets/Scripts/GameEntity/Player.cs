using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing.Utilities;

public class Player : GameEntity
{
    public static float DEFAULT_SPEED = 7.0f;
    public static float DEFAULT_LIFE = 3;
    public static int MAX_SCORE = 999999;
    public static int MAX_SKILL = 8;
    public static int MAX_BULLET_TYPES = 1;
    public static int MAX_SKILL_TYPES = 1;
    public static int REWARD_SKILL_CONDITION = 50;
    public static int REWARD_LIFE_CONDITION = 10000;
    public static float MAX_SKILL_CD_TIME = 2.0f;

    public GameObject[] PlayerBullets = new GameObject[MAX_BULLET_TYPES];
    public GameObject[] PlayerSkills = new GameObject[MAX_SKILL_TYPES];

    private int mScore;
    private int mSkill;
    private int mBulletLevel, mSkillLevel;
    private int mNextRewardScore, mNextRewardLife;
    private float mSkillCDTimer;

    private PostProcessingController ppController;

    public int Score
    {
        set
        {
            mScore = Mathf.Clamp(value, 0, MAX_SCORE);
            RewardSkill();
            RewardLife();
        }
        get { return mScore; }
    }

    public int Skill
    {
        set { mSkill = Mathf.Clamp(value, 0, MAX_SKILL); }
        get { return mSkill; }
    }

    private void InitSelf()
    {
        base.mSpeed = DEFAULT_SPEED;
        base.mLife = DEFAULT_LIFE;
        this.transform.position = new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(-4.0f, 4.0f));
        this.mSkill = 0;
        this.mScore = 0;
        this.mBulletLevel = 0;
        this.mSkillLevel = 0;
        this.mNextRewardScore = REWARD_SKILL_CONDITION;
        this.mNextRewardLife = REWARD_LIFE_CONDITION;
        this.mSkillCDTimer = 0.0f;

        this.ppController = Camera.main.GetComponent<PostProcessingController>();
    }

    protected virtual void Start()
    {
        InitSelf();
    }

    protected virtual void Update()
    {
        if (mLife <= 0)
        {
            AudioManager.Instance.StopMusic();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            // Debug.Log("Life = 0. GAME OVER !");
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

    //技能
    private void Skilling()
    {
        if (mSkillCDTimer > 0.0f)
        {
            mSkillCDTimer -= Time.deltaTime;
            ppController.chromaticAberration.intensity = mSkillCDTimer / MAX_SKILL_CD_TIME;
            ppController.vignette.smoothness = mSkillCDTimer / MAX_SKILL_CD_TIME;
            return;
        }

        if (mSkill > 0 && mSkillCDTimer <= 0.0f && Input.GetMouseButtonDown(1))
        {
            Skill--;
            mSkillCDTimer = MAX_SKILL_CD_TIME;
            //TODO 
            // Instantiate(PlayerSkills[mSkillLevel], this.transform.position, Quaternion.identity);
            // AudioManager.Instance.PlaySound((int) AudioConstant.SHOOT01);
        }
    }

    //奖励技能
    private void RewardSkill()
    {
        if (mScore <= 0) return;

        if (mScore >= mNextRewardScore)
        {
            Skill += (mScore / mNextRewardScore);
            AudioManager.Instance.PlaySound((int) AudioConstant.SKILL_GET);
            mNextRewardScore += REWARD_SKILL_CONDITION;
        }
    }

    //奖励生命
    private void RewardLife()
    {
        if (mScore <= 0) return;

        if (mScore >= mNextRewardLife)
        {
            Skill += (mScore / mNextRewardLife);
            AudioManager.Instance.PlaySound((int) AudioConstant.LIFE_GET);
            mNextRewardLife += REWARD_LIFE_CONDITION;
        }
    }
}