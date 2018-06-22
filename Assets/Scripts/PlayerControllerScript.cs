using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
	public float speed;
	public Rigidbody2D rigidbody;

	private Vector2 mMoveVelocity;

	private void Start()
	{
	}

	private void Update()
	{
		Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		mMoveVelocity = moveInput.normalized * speed;
	}

	private void FixedUpdate() {
		rigidbody.MovePosition(rigidbody.position + mMoveVelocity * Time.fixedDeltaTime);
	}
}