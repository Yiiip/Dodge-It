using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParallaxing : MonoBehaviour
{
	public SpriteRenderer sr;

	private void Update()
	{
		sr.material.SetVector("_PlayerPos", GameWorld.Instance.Player.transform.position);
	}
}