using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class BaseState : MonoBehaviour
{
    // 필드로 설정하여 JsonUtility에서 직렬화 가능하도록 함
    [SerializeField] public int Player_Damage;
    [SerializeField] public int Player_Health;
    [SerializeField] public float Ball_Elasticity;
    [SerializeField] public float Ball_Size;
    [SerializeField] public int Ball_Count;
    [SerializeField] public float Enemy_Health;
    [SerializeField] public float Enemy_Attack;
    [SerializeField] public float Player_Critical_Chance;
    [SerializeField] public float Player_Critical_Damage;
    [SerializeField] public float Player_More_Economy;
    [SerializeField] public bool Ball_Pierce_Power;
    [SerializeField] public bool Ball_Split;
    [SerializeField] public int Pin_Damage;
    [SerializeField] public float Player_Generation;
    [SerializeField] public float Player_DoubleUpChance;

    protected void Awake()
    {
        Player_Damage = 0;
        Player_Health = 0;
        Ball_Elasticity = 0.85f;
        Ball_Size = 0f;
        Ball_Count = 0;
        Enemy_Health = 0;
        Enemy_Attack = 0;
        Player_Critical_Chance = 0f;
        Player_Critical_Damage = 1.5f;
        Player_More_Economy = 0f;
        Ball_Pierce_Power = false;
        Ball_Split = false;
        Pin_Damage = 0;
        Player_Generation = 0f;
        Player_DoubleUpChance = 0f;
    }

    public virtual void AddState(BaseState otherState)
    {
        Player_Damage += otherState.Player_Damage;
        Player_Health += otherState.Player_Health;
        Ball_Elasticity += otherState.Ball_Elasticity;
        Ball_Size += otherState.Ball_Size;
        Ball_Count += otherState.Ball_Count;
        Enemy_Health += otherState.Enemy_Health;
        Enemy_Attack += otherState.Enemy_Attack;
        Player_Critical_Chance += otherState.Player_Critical_Chance;
        Player_Critical_Damage += otherState.Player_Critical_Damage;
        Player_More_Economy += otherState.Player_More_Economy;
        // bool 타입은 누적보다는 true가 하나라도 있으면 true가 되도록 OR 연산을 사용
        Ball_Pierce_Power = Ball_Pierce_Power || otherState.Ball_Pierce_Power;
        Ball_Split = Ball_Split || otherState.Ball_Split;
        Pin_Damage += otherState.Pin_Damage;
        Player_Generation += otherState.Player_Generation;
        Player_DoubleUpChance += otherState.Player_DoubleUpChance;
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