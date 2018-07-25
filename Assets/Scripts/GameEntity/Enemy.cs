using UnityEngine;

public class Enemy : GameEntity {
    
    public string Tag;

    public enum EnemyType
    {
        NORMAL,
        BOSS,
    }

    protected virtual void InitSelf()
    {
        Tag = GetType().Name;
    }
}