using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공통 프로퍼티 구현을 제공하는 추상 클래스
public abstract class BaseState : MonoBehaviour, IState
{
    [SerializeField] private int player_Damage;
    public int Player_Damage
    {
        get => player_Damage;
        set => player_Damage = value;
    }

    [SerializeField] private float player_CriticalChance;
    public float Player_CriticalChance
    {
        get => player_CriticalChance;
        set => player_CriticalChance = value;
    }

    [SerializeField] private int player_HealthIncrease;
    public int Player_HealthIncrease
    {
        get => player_HealthIncrease;
        set => player_HealthIncrease = value;
    }

    [SerializeField] private float player_DoubleUpChance;
    public float Player_DoubleUpChance
    {
        get => player_DoubleUpChance;
        set => player_DoubleUpChance = value;
    }

    [SerializeField] private int player_ShieldPower;
    public int Player_ShieldPower
    {
        get => player_ShieldPower;
        set => player_ShieldPower = value;
    }

    [SerializeField] private float ball_Size;
    public float Ball_Size
    {
        get => ball_Size;
        set => ball_Size = value;
    }

    [SerializeField] private int ball_Count;
    public int Ball_Count
    {
        get => ball_Count;
        set => ball_Count = value;
    }

    [SerializeField] private float ball_Elasticity;
    public float Ball_Elasticity
    {
        get => ball_Elasticity;
        set => ball_Elasticity = value;
    }

    [SerializeField] private int ball_PiercePower;
    public int Ball_PiercePower
    {
        get => ball_PiercePower;
        set => ball_PiercePower = value;
    }

    [SerializeField] private int ball_BallSplitCount;
    public int Ball_BallSplitCount
    {
        get => ball_BallSplitCount;
        set => ball_BallSplitCount = value;
    }
}