using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameEntity
{
    protected const float DEFAULT_SPEED = 7.0f;

    protected override void Start()
    {
        Speed = DEFAULT_SPEED;
    }

    protected override void Update()
    {
        base.Update();
        UpdateVelocityByKeyboard();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected void UpdateVelocityByKeyboard()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = input.normalized * Speed;
    }
}