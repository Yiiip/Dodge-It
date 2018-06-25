using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public EnemyFollower EnemyFollower;
	public Transform[] EnemyFollowerSpawnersPos;

	private float mSpawnTimer;
	private const float ENEMY_FOLLOWER_SPAWN_TIME = 0.5f;

	private Dictionary<int, int> mLevelMaxEnemyCount = new Dictionary<int, int>() {
		{ 1, 10 },
		{ 2, 25 },
	};

	private int mLevel;
	private int mCounter;
	private bool isLevelSpawnFinished;

	void Start()
	{
		mSpawnTimer = ENEMY_FOLLOWER_SPAWN_TIME;
		mLevel = 1;
		mCounter = 0;
		isLevelSpawnFinished = false;
	}

	void Update()
	{
		if (!isLevelSpawnFinished)
		{
			if (mSpawnTimer > 0)
			{
				mSpawnTimer -= Time.deltaTime;
			}
			else
			{
				if (mCounter < mLevelMaxEnemyCount[mLevel])
				{
					Instantiate(EnemyFollower, EnemyFollowerSpawnersPos[Random.Range(0, EnemyFollowerSpawnersPos.Length - 1)].position, Quaternion.identity);
					mCounter++;
					mSpawnTimer = ENEMY_FOLLOWER_SPAWN_TIME;
				}
				else
				{
					isLevelSpawnFinished = true;
				}
			}
		}
		else
		{
			if (mCounter == 0)
			{
				isLevelSpawnFinished = false;
				mLevel++;
			}
		}
	}

}