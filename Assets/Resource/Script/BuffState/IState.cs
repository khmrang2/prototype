using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공통 인터페이스 정의
public interface IState
{
    int Player_Damage { get; set; }
    float Player_CriticalChance { get; set; }
    int Player_HealthIncrease { get; set; }
    float Player_DoubleUpChance { get; set; }
    int Player_ShieldPower { get; set; }

    float Ball_Size { get; set; }
    int Ball_Count { get; set; }
    float Ball_Elasticity { get; set; }
    int Ball_PiercePower { get; set; }
    int Ball_BallSplitCount { get; set; }
}