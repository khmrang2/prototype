using UnityEngine;

[System.Serializable]
public struct EquipmentData
{
    public string name;        // 장비 이름
    public EquipmentType type; // 장비 타입
    public EquipmentRarity rarity; // 장비 등급
    public int mainStat;       // 주 스탯 (공격력, 방어력 등)
    public int subStat;        // 부 스탯 (추가 효과)
    public string icon;        // 아이콘 파일명
    public Sprite iconSprite;  // Unity에서 사용할 아이콘 이미지
    //✅ 
    public EquipmentData(string name, EquipmentType type, EquipmentRarity rarity, int mainStat, int subStat, string icon)
    {
        this.name = name;
        this.type = type;
        this.rarity = rarity;
        this.mainStat = mainStat;
        this.subStat = subStat;
        this.icon = icon;
        
        // Resources 폴더에서 아이콘 불러오기
        this.iconSprite = Resources.Load<Sprite>(icon);
    }
}