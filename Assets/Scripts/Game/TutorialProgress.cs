using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialProgress : MonoBehaviour
{
	public WaveEngine waveEngine;

	bool findLastOne = false;

	private void FixedUpdate()
	{
		if (waveEngine.GetCurrentWave().name == "Begin More" && !findLastOne)
		{
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			if (enemies.Length == 1 && waveEngine.State == WaveEngine.SpawnState.WATTING)
			{
				findLastOne = true;
				enemies[0].AddComponent<TutorialGetSkill>();
			}
		}
		else if (waveEngine.GetCurrentWave().name == "Level 1-1")
		{
			Debug.Log("Tutorial End.");
			Destroy(this);
		}
	}
}