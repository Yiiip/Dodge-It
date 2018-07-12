using UnityEngine;

public class EnemyTailEchoEffect : MonoBehaviour
{
    public GameObject PrefabOfEffect;
    private float timer;
    public float SpawnTime = 0.0f;
    private float destoryTime;

    private EnemyTail enemy;
    private SpriteRenderer mSpriteRenderer;

    private void Start()
    {
        this.destoryTime = Random.Range(0.4f, 1.0f);
        this.enemy = this.GetComponent<EnemyTail>();
        this.mSpriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (timer <= 0)
        {
            GameObject effect = UIUtils.InstantiatePrefab(PrefabOfEffect, this.gameObject.transform);
            effect.transform.localScale = Vector3.one;
            effect.GetComponent<SpriteRenderer>().sprite = mSpriteRenderer.sprite;
            Destroy(effect, destoryTime);
            timer = SpawnTime;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}