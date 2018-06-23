using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : MonoBehaviour
{
    public float Speed;
    public Rigidbody2D Rigidbody;

    protected Vector2 moveVelocity;

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {
        DoMove();
    }

    protected void DoMove()
    {
        Rigidbody.MovePosition(Rigidbody.position + moveVelocity * Time.fixedDeltaTime);
    }
}