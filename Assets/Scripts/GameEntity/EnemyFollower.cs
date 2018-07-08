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
        base.FixedUpdate();
    }

    private void UpdateFollow()
    {
        mMoveVelocity = (mTargetPos.position - this.transform.position).normalized * mSpeed;
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

    protected void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag(mTarget.gameObject.tag))
        {
            mTarget.Life -= 1;
            Instantiate(HitEffect, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}