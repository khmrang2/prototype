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
    [SerializeField] public int Ball_Split;
    [SerializeField] public int Pin_Damage;
    [SerializeField] public float Player_Generation;
    [SerializeField] public float Player_DoubleUpChance;

    protected void Awake()
    {
        Player_Damage = 0;
        Player_Health = 0;
        Ball_Elasticity = 0.85f;
        Ball_Size = 1f;
        Ball_Count = 0;
        Enemy_Health = 0;
        Enemy_Attack = 0;
        Player_Critical_Chance = 0f;
        Player_Critical_Damage = 1.5f;
        Player_More_Economy = 0f;
        Ball_Pierce_Power = false;
        Ball_Split = 0;
        Pin_Damage = 0;
        Player_Generation = 0f;
        Player_DoubleUpChance = 0f;
    }

    public virtual void ApplyBuff(string statName, float value)
    {
        FieldInfo field = typeof(BaseState).GetField(statName);
        if (field == null)
        {
            Debug.LogWarning($"ApplyBuff: 알 수 없는 속성 '{statName}'");
            return;
        }

        if (field.FieldType == typeof(float))
        {
            float currentValue = (float)field.GetValue(this);
            field.SetValue(this, currentValue + value);
        }
        else if (field.FieldType == typeof(int))
        {
            int currentValue = (int)field.GetValue(this);
            field.SetValue(this, currentValue + (int)value);
        }
        else if (field.FieldType == typeof(bool))
        {
            bool currentValue = (bool)field.GetValue(this);
            field.SetValue(this, currentValue || value > 0);
        }
        else
        {
            Debug.LogWarning($"ApplyBuff: 지원되지 않는 속성 유형 '{statName}'");
        }
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