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
        this.mTargetPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        this.mTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        this.mScoreValue = 25;
    }

    protected override void Start()
    {
        base.Start();
        InitSelf();
    }

    protected override void Update()
    {
        base.Update();
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

    protected void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("PlayerBullet"))
        {
            mTarget.Score += mScoreValue;
            Instantiate(HitEffect, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
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