//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;
//using UnityEngine.Networking;

//public struct Buff
//{
//    public int BuffID;
//    public string Name;
//    public string Description;

//    public Buff(int id, string name, string dscrp)
//    {
//        BuffID = id;
//        Name = name;
//        Description = dscrp;
//    }
//}

//public class BuffManager : MonoBehaviour
//{
//    private const string BUFF_JSON_FILENAME = "Buffs.json";
//    private Dictionary<int, Buff> buffDictionary = new Dictionary<int, Buff>();

//    private void Start()
//    {
//        StartCoroutine(LoadBuffsFromJson());
//    }

//    private IEnumerator LoadBuffsFromJson()
//    {
//        string jsonPath = Path.Combine(Application.streamingAssetsPath, BUFF_JSON_FILENAME);
//        Debug.Log("JSON Path: " + jsonPath);

//        string json;
//#if UNITY_ANDROID
//        // Android에서는 UnityWebRequest를 통해 JSON 파일을 불러옵니다.
//        UnityWebRequest request = UnityWebRequest.Get(jsonPath);
//        yield return request.SendWebRequest();

//        if (request.result == UnityWebRequest.Result.Success)
//        {
//            json = request.downloadHandler.text;
//        }
//        else
//        {
//            Debug.LogError("Failed to load JSON: " + request.error);
//            yield break;
//        }
//#else
//            // 다른 플랫폼에서는 File.ReadAllText로 JSON 파일을 불러옵니다.
//            json = File.ReadAllText(jsonPath);
//#endif

//        // JSON 데이터를 Buff[] 배열로 역직렬화합니다.
//        BuffWrapper buffWrapper = JsonUtility.FromJson<BuffWrapper>(json);

//        foreach (var buffData in buffWrapper.buffs)
//        {
//            Buff buff = new Buff(buffData.BuffID, buffData.Name, buffData.Description);
//            buffDictionary.Add(buff.BuffID, buff);
//        }

//        foreach (var buff in buffDictionary.Values)
//        {
//            Debug.Log(buff.Name + ": " + buff.Description + " was loaded\n");
//        }
//    }
//}

//// Buff 배열을 감싸는 래퍼 클래스
//[System.Serializable]
//public class BuffWrapper
//{
//    public Buff[] buffs;
//}
