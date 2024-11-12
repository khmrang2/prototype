using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class BaseState : MonoBehaviour
{
    // 필드로 설정하여 JsonUtility에서 직렬화 가능하도록 함
    public int Player_Damage;
    public float Player_CriticalChance;
    public int Player_HealthIncrease;
    public float Player_DoubleUpChance;
    public int Player_ShieldPower;
    public float Ball_Size;
    public int Ball_Count;
    public float Ball_Elasticity;
    public int Ball_PiercePower;
    public int Ball_BallSplitCount;

    public BaseState()
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

    public void AddState(BaseState otherState)
    {
        Player_Damage += otherState.Player_Damage;
        Player_CriticalChance += otherState.Player_CriticalChance;
        Player_HealthIncrease += otherState.Player_HealthIncrease;
        Player_DoubleUpChance += otherState.Player_DoubleUpChance;
        Player_ShieldPower += otherState.Player_ShieldPower;

        Ball_Size += otherState.Ball_Size;
        Ball_Count += otherState.Ball_Count;
        Ball_Elasticity += otherState.Ball_Elasticity;
        Ball_PiercePower += otherState.Ball_PiercePower;
        Ball_BallSplitCount += otherState.Ball_BallSplitCount;
    }
    // 디버그용 printAllStates.
    public void printAllStates()
    {
        FieldInfo[] fields = this.GetType().GetFields();
        foreach (FieldInfo field in fields)
        {
            Debug.Log($"{field.Name}: {field.GetValue(this)}");
        }
    }
}