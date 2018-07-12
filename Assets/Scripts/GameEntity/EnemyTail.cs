using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTail : GameEntity
{
    public static float DEFAULT_SPEED = 5.0f;
    public static float MAX_LIFE = 1;

    public GameObject HitEffect;
    protected Player mTarget;

    protected int mScoreValue;
    protected Vector2 mDirVec;

    public bool RandomSprite = true;
    protected SpriteRenderer mSpriteRenderer;
    protected string[] mSprites = {
        "Sprites/sEnemyBlue",
        "Sprites/sEnemyRed",
        "Sprites/sEnemyYellow",
        "Sprites/sEnemyFollower",
    };

    protected void InitSelf()
    {
        base.mSpeed = DEFAULT_SPEED + Random.Range(0, 5);
        base.mLife = MAX_LIFE;
        this.mScoreValue = 30;
        this.mTarget = GameWorld.Instance.Player;
        do
        {
            this.mDirVec = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        } while (this.mDirVec.x == 0.0f || this.mDirVec.y == 0.0f);
        this.mSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        if (RandomSprite)
        {
            this.mSpriteRenderer.sprite = (Resources.Load(mSprites[Random.Range(0, mSprites.Length * 5) % mSprites.Length]) as GameObject).GetComponent<SpriteRenderer>().sprite;
        }
    }

    protected virtual void Start()
    {
        InitSelf();
    }

    protected virtual void Update()
    {
        mMoveVelocity = mDirVec * mSpeed;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            if (--mLife <= 0)
            {
                Instantiate(HitEffect, this.transform.position, this.transform.rotation);
                mTarget.Score += mScoreValue;
                Destroy(this.gameObject);
            }
            Destroy(other.gameObject);
        }
    }

    private Collision2D mLastMapBound;
    protected void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag(mTarget.gameObject.tag))
        {
            mTarget.Life -= 1;
            Instantiate(HitEffect, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }

        if (other.collider.tag.Equals("MapBound"))
        {
            if (mLastMapBound == null)
            {
                mLastMapBound = other;
            }
            else
            {
                if (mLastMapBound == other)
                {
                    return;
                }
            }
            ContactPoint2D contactPoint = other.contacts[0];
            // Debug.Log("法线" + contactPoint.normal);
            // Debug.Log("反弹前" + mDirVec);
            mDirVec = Vector2.Reflect(mDirVec, contactPoint.normal); //反弹
            // Debug.Log("反弹后" + mDirVec);
        }
    }
}