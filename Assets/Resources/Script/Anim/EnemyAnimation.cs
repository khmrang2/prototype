using UnityEngine;

public enum EnemyAnimationType
{
    Move,
    Attack,
    Hitted,
    Death
}

[System.Serializable]
public class EnemyAnimationData
{
    public EnemyAnimationType type;
    public string triggerName;
    public AnimationClip clip;
}