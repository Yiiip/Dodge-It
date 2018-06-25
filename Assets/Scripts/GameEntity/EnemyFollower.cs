using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollower : GameEntity
{
    public static float DEFAULT_SPEED = 2.0f;
    public static float MAX_LIFE = 2;

    private Transform mTargetPos;

    private void InitSelf()
    {
        base.mSpeed = DEFAULT_SPEED;
        base.mLife = MAX_LIFE;
        this.mTargetPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
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
}