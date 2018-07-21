using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollower : GameEntity
{
    public static float DEFAULT_SPEED = 2.0f;
    public static float MAX_LIFE = 2;

    protected Transform mTargetPos;
    protected Player mTarget;
    protected int mScoreValue;

    // protected bool isDead = false;
    // protected Queue<GameObject> mEffectQueue = new Queue<GameObject>();

    protected void InitSelf()
    {
        base.mSpeed = DEFAULT_SPEED;
        base.mLife = MAX_LIFE;
        this.mScoreValue = 25;
        this.mTarget = GameWorld.Instance.Player;
        this.mTargetPos = mTarget.gameObject.transform;
    }

    protected virtual void Awake() {
        InitSelf();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        UpdateFollow();
    }

    protected override void FixedUpdate()
    {
        // if (isDead)
        // {
        //     if (mEffectQueue.Count == 0)
        //     {
        //         mEffectQueue.Clear();
        //         DestroyImmediate(this.gameObject);
        //     }
        //     else
        //     {
        //         for (int i = 0; i < mEffectQueue.Count; i++)
        //         {
        //             if (mEffectQueue.Peek().GetComponent<ParticleSystem>().isStopped)
        //             {
        //                 Destroy(mEffectQueue.Dequeue());
        //             }
        //         }
        //     }
        //     return;
        // }

        base.FixedUpdate();
    }

    private void UpdateFollow()
    {
        mMoveVelocity = (mTargetPos.position - this.transform.position).normalized * mSpeed;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        // 碰到子弹
        if (other.CompareTag("PlayerBullet"))
        {
            Camera.main.GetComponent<CameraShaker>().ShakeCameraWithCount(); //震屏

            //按照生命值比例大小生成粒子
            Vector3 scaleByLife = Vector3.one * (1.0f - mLife / MAX_LIFE);
            GameWorld.Instance.HitEffectPool.PopEffect(this.transform.position, scaleByLife, this.transform.rotation);
            //销毁子弹
            Destroy(other.gameObject);
            
            if (--mLife <= 0)
            {
                mTarget.Score += mScoreValue; //打死得分
                Destroy(this.gameObject);
            }
        }
    }

    protected void OnCollisionEnter2D(Collision2D other)
    {
        // 碰到Player
        if (other.collider.CompareTag(mTarget.gameObject.tag))
        {
            mTarget.Life -= 1;
            GameWorld.Instance.HitEffectPool.PopEffect(this.transform.position, Vector3.one, this.transform.rotation);
            Camera.main.GetComponent<CameraShaker>().ShakeCameraWithCount(16); //震屏
        }
    }
}