using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : GameEntity
{
    public static float DEFAULT_SPEED = 7.0f;
    public static float DEFAULT_LIFE = 3;
    public static int MAX_SCORE = 999999;
    public static int MAX_SKILL = 5;
    public static int MAX_BULLET_TYPES = 1;

    public GameObject[] PlayerBullets = new GameObject[MAX_BULLET_TYPES];

    private int mScore;
    private int mSkill;
    private int mBulletLevel;

    public int Score
    {
        set { mScore = Mathf.Clamp(value, 0, MAX_SCORE); }
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
            // Debug.Log("Life = 0!");
        }

        UpdateVelocityByKeyboard();
        Shooting();
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

    private void Shooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(PlayerBullets[mBulletLevel], this.transform.position, Quaternion.identity);
        }
    }
}