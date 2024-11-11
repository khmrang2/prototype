using UnityEngine;


// 버프된 상태를 관리하기 위한 버프 스테이트
public class BuffState : BaseState
{
    private void Awake()
    {
        Player_Damage = 1;
        Player_CriticalChance = 0f;
        Player_HealthIncrease = 0;
        Player_DoubleUpChance = 0f;
        Player_ShieldPower = 0;

        Ball_Size = 0.1f;
        Ball_Count = 2;
        Ball_Elasticity = 0.1f;
        Ball_PiercePower = 50;
        Ball_BallSplitCount = 1;
    }
}