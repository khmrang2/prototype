using UnityEngine;

// ✅ 장비 타입 (무기, 기어, 심장)
public enum EquipmentType
{
    Weapon,
    Gear,
    Heart
}

// ✅ 장비 등급 (커먼~레전더리)
public enum EquipmentRarity
{
    Common,     // 50%
    Uncommon,   // 30%
    Rare,       // 15%
    Epic,       // 4%
    Legendary   // 1%
}