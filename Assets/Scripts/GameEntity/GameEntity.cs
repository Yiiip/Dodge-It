using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : MonoBehaviour
{
    [SerializeField]
    protected float mSpeed;
    protected Vector2 mMoveVelocity;
    protected float mLife;

    protected virtual void FixedUpdate()
    {
        DoMove();
    }

    protected void DoMove()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody != null)
        {
            rigidbody.MovePosition(rigidbody.position + mMoveVelocity * Time.fixedDeltaTime);
        }
    }

    public float Life
    {
        set { mLife = value; }
        get { return mLife; }
    }
}