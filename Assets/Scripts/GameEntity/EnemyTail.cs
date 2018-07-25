using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTail : Enemy
{
    public static float DEFAULT_SPEED = 5.0f;
    public static float MAX_LIFE = 1;

    protected Player mTarget;

    protected int mScoreValue;
    protected Vector2 mDirVec;

    public bool RandomSprite = true;
    protected SpriteRenderer mSpriteRenderer;
    protected string[] mSprites = {
        "Sprites/sEnemyBlue",
        "Sprites/sEnemyRed",
        "Sprites/sEnemyYellow",
        "Sprites/sEnemyGreen",
    };

    protected override void InitSelf()
    {
        base.InitSelf();
        base.mSpeed = DEFAULT_SPEED + Random.Range(-1, 6);
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
                mTarget.Score += mScoreValue;
                GameWorld.Instance.HitEffectPool.PopEffect(this.transform.position, Vector3.one, this.transform.rotation);
                Camera.main.GetComponent<CameraShaker>().ShakeCameraWithCount(); //震屏
                AudioManager.Instance.PlaySound((int) AudioConstant.ENEMY_DESTORY01);
                Destroy(this.gameObject);
            }
            else
            {
                AudioManager.Instance.PlaySound((int) AudioConstant.HIT01);
            }
            Destroy(other.gameObject);
        }
    }

    private Collision2D mLastMapBound;
    protected void OnCollisionEnter2D(Collision2D other)
    {
        // 碰到Player
        if (other.collider.CompareTag(mTarget.gameObject.tag))
        {
            mTarget.Life -= 1;
            GameWorld.Instance.HitEffectPool.PopEffect(this.transform.position, Vector3.one, this.transform.rotation);
            Camera.main.GetComponent<CameraShaker>().ShakeCameraWithCount(16); //震屏
            AudioManager.Instance.PlaySound((int) AudioConstant.ENEMY_DESTORY01);
            Destroy(this.gameObject);
        }

        // 碰到墙壁
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