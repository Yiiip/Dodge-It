using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameWorld : MonoBehaviour
{
    private static GameWorld sInstance;

    [SerializeField] private EGameState mState;
    [SerializeField] public Player Player;
    [SerializeField] public WaveEngine WaveEngine;
    [SerializeField] private PostProcessVolume mPostProcessVolume;
    [HideInInspector] public PostProcessProfile postProcessProfile;
    [SerializeField] public GameObject EffectPools;
    private EffectPool mHitEffectPool;
    private EffectPool mCollideBoundEffectPool;

    public static GameWorld Instance
    {
        get {  return sInstance; }
    }

    public EGameState State
    {
        get { return mState; }
        set { mState = value; }
    }

    public EffectPool HitEffectPool
    {
        get { return mHitEffectPool; }
    }
    public EffectPool CollideBoundEffectPool
    {
        get { return mCollideBoundEffectPool; }
    }

    private void Awake() {
        if (sInstance == null)
        {
            sInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start() {
        mState = EGameState.MAIN_MENU;
        postProcessProfile = mPostProcessVolume.profile;

        EffectPool[] effectPools = EffectPools.GetComponents<EffectPool>();
        for (int i = 0; i < effectPools.Length; i++)
        {
            if (effectPools[i].PoolTag.Equals("Hit"))
            {
                mHitEffectPool = effectPools[i];
            }
            else if (effectPools[i].PoolTag.Equals("WithBounds"))
            {
                mCollideBoundEffectPool = effectPools[i];
            }
        }

        UIUtils.SetVisibility(Player.gameObject, false);

        PlayBGM();
    }

    private void PlayBGM()
    {
        AudioManager.Instance.PlayMusic(0, true, 0.0f);
    }

    private void Update() {
        if (mState == EGameState.PLAYING)
        {
            if (!Player.gameObject.activeSelf && !WaveEngine.gameObject.activeSelf)
            {
                UIUtils.SetVisibility(Player.gameObject, true);
                UIUtils.SetVisibility(WaveEngine.gameObject, true);
            }
        }
    }

    private void OnDestroy() {
        sInstance = null;
    }
}

public enum EGameState
{
    MAIN_MENU,
    PLAYING
}