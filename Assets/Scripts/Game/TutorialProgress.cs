using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialProgress : MonoBehaviour
{
	public WaveEngine waveEngine;

	private void FixedUpdate()
	{
		if (waveEngine.GetCurrentWave().name == "Level 1-1")
		{
			Debug.Log("教学结束！");
			//TODO 掉落技能
			Destroy(this);
		}
	}
}