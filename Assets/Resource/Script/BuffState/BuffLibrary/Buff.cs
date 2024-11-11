using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffEffect : BaseState
{
    public int ID;
}

public class Buff : MonoBehaviour
{
    private Dictionary<int, BuffEffect> buffTable = new Dictionary<int, BuffEffect>();

    [SerializeField] private TextAsset buffDataJSON; // JSON ������ Unity�� �߰�
    private BuffEffect currentBuff;
    public BuffManager buffManager;

    private void Start()
    {
        LoadBuffDataFromJSON();
        buffManager.addBuff(currentBuff);
    }

    private void LoadBuffDataFromJSON()
    {
        BuffDataList dataList = JsonUtility.FromJson<BuffDataList>(buffDataJSON.text);
        
        // json ���Ͽ��� ��� �������� �ε��ؿ�.
        foreach (BuffEffect buff in dataList.buffs)
        {
            buffTable[buff.ID] = buff;
        }
    }

    // Ư�� ID�� �ش��ϴ� ���� �����͸� �ε�
    // �� �гο��� Ŭ���ϸ� �̰Ÿ� ȣ���ϰ� �ϸ� ��. �̰� �� Ŭ�� �޼ҵ��.
    public void LoadBuffById(int buffId)
    {
        buffTable.TryGetValue(buffId, out currentBuff);
        
    }

    // buffState�� ���� ����
    //public void ApplyBuff(BuffState buffState)
    //{
    //    if (currentBuffEffect == null) return;

    //    // nullable Ÿ���̱� ������ �̷��� ǥ��.
    //    if (currentBuffEffect.Player_Damage.HasValue)
    //        buffState.Player_Damage += currentBuffEffect.Player_Damage.Value;

    //    if (currentBuffEffect.Player_CriticalChance.HasValue)
    //        buffState.Player_CriticalChance += currentBuffEffect.Player_CriticalChance.Value;

    //    if (currentBuffEffect.Player_HealthIncrease.HasValue)
    //        buffState.Player_HealthIncrease += currentBuffEffect.Player_HealthIncrease.Value;

    //    if (currentBuffEffect.Player_DoubleUpChance.HasValue)
    //        buffState.Player_DoubleUpChance += currentBuffEffect.Player_DoubleUpChance.Value;

    //    if (currentBuffEffect.Player_ShieldPower.HasValue)
    //        buffState.Player_ShieldPower += currentBuffEffect.Player_ShieldPower.Value;

    //    if (currentBuffEffect.Ball_Size.HasValue)
    //        buffState.Ball_Size += currentBuffEffect.Ball_Size.Value;

    //    if (currentBuffEffect.Ball_Count.HasValue)
    //        buffState.Ball_Count += currentBuffEffect.Ball_Count.Value;

    //    if (currentBuffEffect.Ball_Elasticity.HasValue)
    //        buffState.Ball_Elasticity += currentBuffEffect.Ball_Elasticity.Value;

    //    if (currentBuffEffect.Ball_PiercePower.HasValue)
    //        buffState.Ball_PiercePower += currentBuffEffect.Ball_PiercePower.Value;

    //    if (currentBuffEffect.Ball_BallSplitCount.HasValue)
    //        buffState.Ball_BallSplitCount += currentBuffEffect.Ball_BallSplitCount.Value;
    //}
}

[System.Serializable]
public class BuffDataList
{
    public List<BuffEffect> buffs;
}