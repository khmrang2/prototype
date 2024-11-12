using UnityEngine;


// ������ ���¸� �����ϱ� ���� ���� ������Ʈ
public class BuffState : BaseState
{
    private void Awake()
    {
        Player_Damage = 0;
        Player_CriticalChance = 0f;
        Player_HealthIncrease = 0;
        Player_DoubleUpChance = 0f;
        Player_ShieldPower = 0;

        Ball_Size = 0f;
        Ball_Count = 0;
        Ball_Elasticity = 0f;
        Ball_PiercePower = 0;
        Ball_BallSplitCount = 0;
    }
}