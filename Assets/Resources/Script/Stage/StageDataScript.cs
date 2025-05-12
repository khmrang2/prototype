using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataList", menuName = "Game/Stage Database")]
public class StageDataScript : ScriptableObject
{
    public GameObject stagePrefab;
    public List<StageData> stages;
    
    /// <summary>
    /// 스테이지 번호로 스테이지 데이터를 찾아 반환합니다.
    /// </summary>
    public StageData GetStageByNumber(int stageNumber)
    {
        foreach (StageData stage in stages)
        {
            if (stage.stageNumber == stageNumber)
            {
                return stage;
            }
        }
        
        // 찾지 못한 경우 기본값 반환
        Debug.LogWarning($"스테이지 번호 {stageNumber}에 해당하는 데이터를 찾을 수 없습니다.");
        return default;
    }
    
    /// <summary>
    /// 스테이지 이름으로 스테이지 데이터를 찾아 반환합니다.
    /// </summary>
    public StageData GetStageByName(string stageName)
    {
        foreach (StageData stage in stages)
        {
            if (stage.stageName == stageName)
            {
                return stage;
            }
        }
        
        // 찾지 못한 경우 기본값 반환
        Debug.LogWarning($"스테이지 이름 {stageName}에 해당하는 데이터를 찾을 수 없습니다.");
        return default;
    }
    
    /// <summary>
    /// 스테이지 데이터 목록의 크기를 반환합니다.
    /// </summary>
    public int GetStageCount()
    {
        return stages != null ? stages.Count : 0;
    }
}

[System.Serializable]
public struct StageData
{
    public string stageName;
    public int stageNumber;
    public Sprite stageThumbnail;
}