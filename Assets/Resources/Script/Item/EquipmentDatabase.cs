using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class EquipmentDatabase : MonoBehaviour
{
    public static Dictionary<(EquipmentType, EquipmentRarity), EquipmentData> stats = new Dictionary<(EquipmentType, EquipmentRarity), EquipmentData>();

    private static Dictionary<EquipmentRarity, float> rarityProbabilities = new Dictionary<EquipmentRarity, float>
    {
        { EquipmentRarity.Common, 50f },
        { EquipmentRarity.Uncommon, 30f },
        { EquipmentRarity.Rare, 15f },
        { EquipmentRarity.Epic, 4f },
        { EquipmentRarity.Legendary, 1f }
    };

    // ✅ JSON 데이터 파싱을 위한 클래스 추가
    [System.Serializable]
    public class EquipmentJson
    {
        public string name;
        public string type;
        public string rarity;
        public int mainStat;
        public int subStat;
        public string icon;
    }

    [System.Serializable]
    public class EquipmentJsonWrapper
    {
        public EquipmentJson[] items;
    }

    void Awake()
    {
		Debug.Log("코루틴 실행됨 ㅇㅇ");
        StartCoroutine(LoadEquipmentData());
    }

    IEnumerator LoadEquipmentData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "equipment_data.json");
        Debug.Log($"🔍 JSON 파일 경로 확인: {filePath}");

        string jsonText = "";

        // ✅ Windows, Mac 환경에서는 File.ReadAllText() 사용 가능
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"🚨 JSON 파일을 찾을 수 없습니다! 경로: {filePath}");
                yield break;
            }
            jsonText = File.ReadAllText(filePath);
        }
        else
        {
            // ✅ Android & iOS는 UnityWebRequest로 읽어야 함
            UnityWebRequest request = UnityWebRequest.Get(filePath);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"🚨 JSON 파일을 읽을 수 없습니다! {request.error}");
                yield break;
            }

            jsonText = request.downloadHandler.text;
        }

        if (string.IsNullOrEmpty(jsonText))
        {
            Debug.LogError("🚨 JSON 파일이 비어 있습니다!");
            yield break;
        }

        // ✅ JSON 내용 출력
        Debug.Log($"✅ JSON 데이터 로드 성공! 내용: {jsonText}");

        EquipmentJsonWrapper wrapper;
        try
        {
            wrapper = JsonUtility.FromJson<EquipmentJsonWrapper>(jsonText);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"🚨 JSON 파싱 중 오류 발생! {e.Message}");
            yield break;
        }

        if (wrapper == null || wrapper.items == null || wrapper.items.Length == 0)
        {
            Debug.LogError("🚨 JSON 데이터 파싱 실패! 파일 내용을 확인하세요.");
            yield break;
        }

        // ✅ 장비 데이터를 Dictionary에 추가
        int addedCount = 0;
        foreach (var item in wrapper.items)
        {
            Debug.Log($"📌 JSON 데이터 파싱: name={item.name}, type={item.type}, rarity={item.rarity}, mainStat={item.mainStat}, subStat={item.subStat}");

            if (System.Enum.TryParse(item.type, out EquipmentType type) &&
                System.Enum.TryParse(item.rarity, out EquipmentRarity rarity))
            {
                EquipmentData data = new EquipmentData(item.name, type, rarity, item.mainStat, item.subStat, item.icon);
                stats[(type, rarity)] = data;
                addedCount++;
            }
            else
            {
                Debug.LogError($"🚨 Enum 변환 실패! type={item.type}, rarity={item.rarity}");
            }
        }

        Debug.Log($"✅ 장비 데이터 JSON 로드 완료! 로드된 장비 개수: {addedCount}");
    }

    public static List<EquipmentData> GetRandomEquipment(int count)
    {
        List<EquipmentData> equipmentList = new List<EquipmentData>();

        if (stats.Count == 0)
        {
            Debug.LogError("🚨 장비 데이터가 없습니다! JSON 파일을 확인하세요.");
            return equipmentList;
        }

        List<EquipmentData> allItems = new List<EquipmentData>(stats.Values);
        for (int i = 0; i < count; i++)
        {
            if (allItems.Count > 0)
            {
                int randomIndex = Random.Range(0, allItems.Count);
                equipmentList.Add(allItems[randomIndex]);
            }
        }

        return equipmentList;
    }
}
