using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WaterParallaxing : MonoBehaviour
{
	private SpriteRenderer sr;

	private void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		sr.material.SetVector("_PlayerPos", GameWorld.Instance.Player.transform.position);
	}
}