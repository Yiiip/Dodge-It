using System.Collections;
using UnityEngine;

public class WaveEngine : MonoBehaviour {
    
    public enum SpawnState
    {
        SPAWNING, WATTING, COUNTING,
    }

    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameEntity enemy;
        public int count;
        public float rate;
        public int difficulty;
    }

    public Wave[] waves;
    public Transform[] spawnPositions;
    protected int nextWave = 0;

    public float timeBetweenWaves = 5.0f;
    protected float waveTimer;
    protected float searchAliveTimer = 1.0f;
    protected SpawnState state;

    protected void Start() {
        waveTimer = timeBetweenWaves;
        state = SpawnState.COUNTING;

        if (spawnPositions.Length == 0)
        {
            Debug.LogError("Spawn positions array is empty!");
        }
    }

    protected void Update() {
        if (state == SpawnState.WATTING)
        {
            if (!IsEnemyAlive())
            {
                //Begin a new round
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveTimer <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveTimer -= Time.deltaTime;
        }
    }

    protected bool IsEnemyAlive()
    {
        searchAliveTimer -= Time.deltaTime;

        if (GameObject.FindGameObjectWithTag("Enemy") == null
            && searchAliveTimer <= 0.0f)
        {
            searchAliveTimer = 1.0f;
            return false;
        }
        return true;
    }

    protected IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("-> Wave " + nextWave + ": " + wave.name);
        state = SpawnState.SPAWNING;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy, wave.difficulty);
            yield return new WaitForSeconds(1.0f / wave.rate);
        }

        state = SpawnState.WATTING;
        yield break;
    }

    protected void SpawnEnemy(GameEntity enemy, int difficulty)
    {
        Transform spawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)];
        GameEntity e = Instantiate(enemy, spawnPos.position, spawnPos.rotation);
        if (e is EnemyFollower)
        {
            e.Speed = e.Speed + difficulty * 0.4f;
        }
    }

    protected void WaveCompleted()
    {
        Debug.Log("Wave " + nextWave + " completed!");
        state = SpawnState.COUNTING;
        waveTimer = timeBetweenWaves;
        nextWave = (nextWave + 1) % waves.Length;
    }
}