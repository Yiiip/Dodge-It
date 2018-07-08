using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    private static GameWorld sInstance;

    private GameState mState;
    public Player Player;
    public WaveEngine WaveEngine;

    public static GameWorld Instance
    {
        get {  return sInstance; }
    }

    public GameState State
    {
        get { return mState; }
        set { mState = value; }
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
        mState = GameState.MAIN_MENU;
        PlayBGM(); 
    }

    private void PlayBGM()
    {
        AudioManager.Instance.PlayMusic(0, true, 0.0f);
    }

    private void Update() {
        if (mState == GameState.PLAYING)
        {
            if (!Player.gameObject.active && !WaveEngine.gameObject.active)
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

public enum GameState
{
    MAIN_MENU,
    PLAYING
}