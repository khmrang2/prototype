using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    //private Dictionary<int, BuffEffect> buffTable = new Dictionary<int, BuffEffect>();

    //[SerializeField] private TextAsset buffDataJSON; // JSON ������ Unity�� �߰�
    //private BuffEffect currentBuff;
    //public BuffManager buffManager;

    //private void Start()
    //{
    //    LoadBuffDataFromJSON();
    //}

    //private void LoadBuffDataFromJSON()
    //{
    //    BuffDataList dataList = JsonUtility.FromJson<BuffDataList>(buffDataJSON.text);

    //    // json ���Ͽ��� ��� �������� �ε��ؿ�.
    //    foreach (BuffEffect buff in dataList.buffs)
    //    {
    //        buffTable[buff.ID] = buff;
    //    }
    //}

    //// Ư�� ID�� �ش��ϴ� ���� �����͸� �ε�
    //// �� �гο��� Ŭ���ϸ� �̰Ÿ� ȣ���ϰ� �ϸ� ��. �̰� �� Ŭ�� �޼ҵ��.
    //public void LoadBuffById(int buffId)
    //{
    //    buffTable.TryGetValue(buffId, out currentBuff);
    //    buffManager.addBuff(currentBuff);
    //}

    //// ��ư�� ������ �� ���� ID�� LoadBuffById�� ȣ���ϴ� �޼���
    //public void OnButtonPress()
    //{
    //    int randomId = Random.Range(1, 3); // 1���� 100 ������ ���� ID ���� (JSON�� ID ������ ���߱�)
    //    LoadBuffById(randomId);
    //}
}

//[System.Serializable]
//public class BuffDataList
//{
//    public List<BuffEffect> buffs;
//}