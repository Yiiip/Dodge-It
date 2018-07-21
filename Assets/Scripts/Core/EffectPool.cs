using UnityEngine;

public class EffectPool : MonoBehaviour
{
    [SerializeField] public string PoolTag;

    [Tooltip("Allow pool to grow as needed (if checked)")]
    [SerializeField] bool dynamicPool = false;
    [SerializeField] int initialPoolSize = 10;
    [Range(1, 5)][SerializeField] int poolGrowthRate = 2;

    [Space(10)][SerializeField] GameObject effectPrefab;
    [SerializeField] float effectDuration = 1;

    private int dynamicPoolSize;
    private int poolIndex;

    struct Effect
    {
        public bool on;
        public float onTime;
        public GameObject effectObject;
    }

    Effect[] effects;

    private void Start()
    {
        if (PoolTag.Equals("") || PoolTag == null) Debug.LogError(GetType().Name + " PoolTag is empty!");

        poolIndex = initialPoolSize;
        dynamicPoolSize = initialPoolSize;
        effects = new Effect[initialPoolSize];

        for (int i = 0; i < initialPoolSize; i++) CreateEffect(i);
    }

    private void Update()
    {
        for (int i = 0; i < dynamicPoolSize; i++)
        {
            if (effects[i].on)
            {
                if (Time.time > effects[i].onTime + effectDuration)
                {
                    effects[i].on = false;
                    effects[i].effectObject.SetActive(false);
                }
            }
        }
    }

    private void CreateEffect(int index)
    {
        effects[index].effectObject = Instantiate(effectPrefab, transform.position, Quaternion.identity, transform);
        effects[index].onTime = 0;
        effects[index].on = false;
        effects[index].effectObject.SetActive(false);
    }

    private void GrowPool()
    {
        Effect[] temp = new Effect[dynamicPoolSize];
        for (int i = 0; i < dynamicPoolSize; i++)
        {
            temp[i] = effects[i];
        }
        dynamicPoolSize += poolGrowthRate;

        effects = new Effect[dynamicPoolSize];
        for (int i = 0; i < dynamicPoolSize - poolGrowthRate; i++)
        {
            effects[i] = temp[i];
        }
        for (int i = 0; i < poolGrowthRate; i++)
        {
            CreateEffect(i + dynamicPoolSize - poolGrowthRate);
        }
    }

    public void PopEffect(Vector3 position, Vector3 scale, Quaternion rotation)
    {
        if (++poolIndex >= dynamicPoolSize)
        {
            poolIndex = 0;
        }

        if (effects[poolIndex].on && dynamicPool)
        {
            GrowPool();
            poolIndex++;
        }

        RecycleEffect(position, scale, rotation, poolIndex);
    }

    private void RecycleEffect(Vector3 position, Vector3 scale, Quaternion rotation, int index)
    {
        effects[index].on = true;
        effects[index].onTime = Time.time;
        effects[index].effectObject.transform.position = position;
        effects[index].effectObject.transform.localScale = scale;
        effects[index].effectObject.transform.rotation = rotation;

        // doing a pre-emtive toggle-off ensures that even non-dynamic use gives a
        // more dynamic appearance, but depending on your application, it might not
        // be optimal -- it's safe to comment-out the following "false" line.
        effects[index].effectObject.SetActive(false);
        effects[index].effectObject.SetActive(true);
    }
}