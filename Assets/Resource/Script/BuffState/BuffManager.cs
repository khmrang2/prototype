using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public GameManager gameManager; // ���� �Ŵ��� ����


    PlayerState playerDefaultState = null;

    [SerializeField] private BuffState buffState; // buffState ����
    private List<BuffEffect> allBuffs = new List<BuffEffect>(); // ��� Ȱ��ȭ�� ����
    private List<BuffEffect> newBuffs = new List<BuffEffect>(); // ���� �߰��� ����

    private void Start()
    {
        // �÷��̾� ������Ʈ�� ���� ���� �ؼ� ������.
        // => json���� load�ؼ� �÷��̾��� ������ �������� �������� �ٲ� �� ����.
        playerDefaultState = new PlayerState();
    }

    // �� ������ �߰��ϰ� newBuffs�� ����
    public void addBuff(BuffEffect buff)
    {
        newBuffs.Add(buff);
    }

    // �� ������ ���ؼ� ������Ʈ ���־� buffState�� ������Ʈ.
    public BuffState updateBuffState()
    {
        foreach (BuffEffect buff in newBuffs)
        {
            ApplyBuffEffectToPlayer(buff);
            allBuffs.Add(buff); // ������ ������ allBuffs�� �̵�
        }

        newBuffs.Clear(); // newBuffs �ʱ�ȭ
        return buffState;
    }

    // ���ڷ� ���� ������ ����ϴ� �޼ҵ�
    private void ApplyBuffEffectToPlayer(BuffEffect buff)
    {
        if (buff.Player_Damage.HasValue)
            buffState.Player_Damage += buff.Player_Damage.Value;

        if (buff.Player_CriticalChance.HasValue)
            buffState.Player_CriticalChance += buff.Player_CriticalChance.Value;

        if (buff.Player_HealthIncrease.HasValue)
            buffState.Player_HealthIncrease += buff.Player_HealthIncrease.Value;

        if (buff.Player_DoubleUpChance.HasValue)
            buffState.Player_DoubleUpChance += buff.Player_DoubleUpChance.Value;

        if (buff.Player_ShieldPower.HasValue)
            buffState.Player_ShieldPower += buff.Player_ShieldPower.Value;

        if (buff.Ball_Size.HasValue)
            buffState.Ball_Size += buff.Ball_Size.Value;

        if (buff.Ball_Count.HasValue)
            buffState.Ball_Count += buff.Ball_Count.Value;

        if (buff.Ball_Elasticity.HasValue)
            buffState.Ball_Elasticity += buff.Ball_Elasticity.Value;

        if (buff.Ball_PiercePower.HasValue)
            buffState.Ball_PiercePower += buff.Ball_PiercePower.Value;

        if (buff.Ball_BallSplitCount.HasValue)
            buffState.Ball_BallSplitCount += buff.Ball_BallSplitCount.Value;
    }

    public BuffState getBuffState()
    {
        return this.buffState;
    }
}