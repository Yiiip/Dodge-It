using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollower : GameEntity
{
    public static float DEFAULT_SPEED = 2.0f;
    public static float MAX_LIFE = 2;

    public GameObject HitEffect;

    protected Transform mTargetPos;
    protected Player mTarget;
    protected int mScoreValue;

    protected bool isDead = false;
    protected Queue<GameObject> mEffectQueue = new Queue<GameObject>();

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
        if (isDead)
        {
            if (mEffectQueue.Count == 0)
            {
                mEffectQueue.Clear();
                DestroyImmediate(this.gameObject);
            }
            else
            {
                for (int i = 0; i < mEffectQueue.Count; i++)
                {
                    if (mEffectQueue.Peek().GetComponent<ParticleSystem>().isStopped)
                    {
                        Destroy(mEffectQueue.Dequeue());
                    }
                }
            }
            return;
        }

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
            if (--mLife <= 0)
            {
                mTarget.Score += mScoreValue; //打死才得分
                gameObject.transform.localScale = Vector3.zero; //这里不直接销毁自己，先隐藏，等所以粒子播完再销毁
                isDead = true;
            }

            Camera.main.GetComponent<CameraShaker>().ShakeCameraWithCount(); //震屏

            //按照生命值比例大小生成粒子
            GameObject effect = Instantiate(HitEffect, this.transform.position, this.transform.rotation);
            effect.transform.localScale *= (1.0f - mLife / MAX_LIFE);
            mEffectQueue.Enqueue(effect);
            //销毁子弹
            Destroy(other.gameObject);
        }
    }

    protected void OnCollisionEnter2D(Collision2D other)
    {
        // 碰到Player
        if (other.collider.CompareTag(mTarget.gameObject.tag))
        {
            mTarget.Life -= 1;
            mEffectQueue.Enqueue(Instantiate(HitEffect, this.transform.position, this.transform.rotation));
            gameObject.transform.localScale = Vector3.zero;
            isDead = true;

            Camera.main.GetComponent<CameraShaker>().ShakeCameraWithCount(); //震屏
        }
    }
}