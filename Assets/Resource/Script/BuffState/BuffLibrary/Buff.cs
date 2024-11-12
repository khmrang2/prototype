using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    //private Dictionary<int, BuffEffect> buffTable = new Dictionary<int, BuffEffect>();

    //[SerializeField] private TextAsset buffDataJSON; // JSON 파일을 Unity에 추가
    //private BuffEffect currentBuff;
    //public BuffManager buffManager;

    //private void Start()
    //{
    //    LoadBuffDataFromJSON();
    //}

    //private void LoadBuffDataFromJSON()
    //{
    //    BuffDataList dataList = JsonUtility.FromJson<BuffDataList>(buffDataJSON.text);

    //    // json 파일에서 모든 버프들을 로드해옴.
    //    foreach (BuffEffect buff in dataList.buffs)
    //    {
    //        buffTable[buff.ID] = buff;
    //    }
    //}

    //// 특정 ID에 해당하는 버프 데이터를 로드
    //// 즉 패널에서 클릭하면 이거를 호출하게 하면 됨. 이걸 온 클릭 메소드로.
    //public void LoadBuffById(int buffId)
    //{
    //    buffTable.TryGetValue(buffId, out currentBuff);
    //    buffManager.addBuff(currentBuff);
    //}

    //// 버튼이 눌렸을 때 랜덤 ID로 LoadBuffById를 호출하는 메서드
    //public void OnButtonPress()
    //{
    //    int randomId = Random.Range(1, 3); // 1에서 100 사이의 랜덤 ID 생성 (JSON의 ID 범위에 맞추기)
    //    LoadBuffById(randomId);
    //}
}

//[System.Serializable]
//public class BuffDataList
//{
//    public List<BuffEffect> buffs;
//}