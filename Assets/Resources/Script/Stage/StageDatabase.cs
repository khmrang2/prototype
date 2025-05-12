using UnityEngine;

[CreateAssetMenu(fileName = "StageDatabase", menuName = "Game/Stage Database")]
public class StageDatabase : ScriptableObject
{
    public StageData[] stages;
}