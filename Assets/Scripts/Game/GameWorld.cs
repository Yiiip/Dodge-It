using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    private GameWorld sInstance;

    public GameWorld Instance
    {
        get
        {
            return this.sInstance;
        }
    }

    private void Awake() {
        if (sInstance == null)
        {
            this.sInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy() {
        this.sInstance = null;
    }
}