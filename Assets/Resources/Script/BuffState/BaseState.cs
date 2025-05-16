using System.Reflection;
using UnityEngine;

[System.Serializable]
public class BaseState : MonoBehaviour
{
    [SerializeField] private int _playerDamage;
    [SerializeField] private int _playerHealth;
    [SerializeField] private float _ballElasticity = 0.85f;
    [SerializeField] private float _ballSize = 1f;
    [SerializeField] private int _ballCount;
    [SerializeField] private float _enemyHealth;
    [SerializeField] private float _enemyAttack;
    [SerializeField] private float _playerCriticalChance = 0.1f;
    [SerializeField] private float _playerCriticalDamage = 2.0f;
    [SerializeField] private float _playerMoreEconomy;
    [SerializeField] private bool _ballPiercePower;
    [SerializeField] private int _ballSplit;
    [SerializeField] private int _pinDamage;
    [SerializeField] private float _playerGeneration;
    [SerializeField] private float _playerDoubleUpChance;

    // 프로퍼티들
    public int Player_Damage { get => _playerDamage; set => _playerDamage = value; }
    public int Player_Health { get => _playerHealth; set => _playerHealth = value; }
    public float Ball_Elasticity { get => _ballElasticity; set => _ballElasticity = Mathf.Clamp01(value); }
    public float Ball_Size { get => _ballSize; set => _ballSize = value; }
    public int Ball_Count { get => _ballCount; set => _ballCount = value; }
    public float Enemy_Health { get => _enemyHealth; set => _enemyHealth = value; }
    public float Enemy_Attack { get => _enemyAttack; set => _enemyAttack = value; }
    public float Player_Critical_Chance { get => _playerCriticalChance; set => _playerCriticalChance = value; }
    public float Player_Critical_Damage { get => _playerCriticalDamage; set => _playerCriticalDamage = value; }
    public float Player_More_Economy { get => _playerMoreEconomy; set => _playerMoreEconomy = value; }
    public bool Ball_Pierce_Power { get => _ballPiercePower; set => _ballPiercePower = value; }
    public int Ball_Split { get => _ballSplit; set => _ballSplit = value; }
    public int Pin_Damage { get => _pinDamage; set => _pinDamage = value; }
    public float Player_Generation { get => _playerGeneration; set => _playerGeneration = value; }
    public float Player_DoubleUpChance { get => _playerDoubleUpChance; set => _playerDoubleUpChance = value; }

    protected void Awake()
    {
        Player_Damage = 0;
        Player_Health = 0;
        Ball_Elasticity = 0.85f;
        Ball_Size = 1f;
        Ball_Count = 0;
        Enemy_Health = 0;
        Enemy_Attack = 0;
        Player_Critical_Chance = 0.1f;
        Player_Critical_Damage = 2.0f;
        Player_More_Economy = 0f;
        Ball_Pierce_Power = false;
        Ball_Split = 0;
        Pin_Damage = 0;
        Player_Generation = 0f;
        Player_DoubleUpChance = 0f;
    }

    public virtual void ApplyBuff(string statName, float value)
    {
        PropertyInfo property = typeof(BaseState).GetProperty(statName);
        if (property == null)
        {
            Debug.LogWarning($"ApplyBuff: 알 수 없는 속성 '{statName}'");
            return;
        }

        if (property.PropertyType == typeof(float))
        {
            float currentValue = (float)property.GetValue(this);
            property.SetValue(this, currentValue + value);
        }
        else if (property.PropertyType == typeof(int))
        {
            int currentValue = (int)property.GetValue(this);
            property.SetValue(this, currentValue + (int)value);
        }
        else if (property.PropertyType == typeof(bool))
        {
            bool currentValue = (bool)property.GetValue(this);
            property.SetValue(this, currentValue || value > 0);
        }
        else
        {
            Debug.LogWarning($"ApplyBuff: 지원되지 않는 속성 유형 '{statName}'");
        }
    }

    public void printAllStates()
    {
        PropertyInfo[] properties = this.GetType().GetProperties();
        foreach (PropertyInfo property in properties)
        {
            Debug.Log($"{property.Name}: {property.GetValue(this)}");
        }
    }
}