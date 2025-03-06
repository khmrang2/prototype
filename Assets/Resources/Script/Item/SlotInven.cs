using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotInven : MonoBehaviour
{
    public Image backgroundImage;

    public Sprite commonSprite;    // rarity 0
    public Sprite uncommonSprite;  // rarity 1
    public Sprite rareSprite;      // rarity 2
    public Sprite epicSprite;      // rarity 3
    public Sprite legendarySprite; // rarity 4

    private void Awake()
    {

    }

    /// <summary>
    /// rarity ���� ���� ��� �̹����� �����մϴ�.
    /// </summary>
    public void SetRarity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                backgroundImage.sprite = commonSprite;
                break;
            case Rarity.Uncommon:
                backgroundImage.sprite = uncommonSprite;
                break;
            case Rarity.Rare:
                backgroundImage.sprite = rareSprite;
                break;
            case Rarity.Epic:
                backgroundImage.sprite = epicSprite;
                break;
            case Rarity.Legendary:
                backgroundImage.sprite = legendarySprite;
                break;
            default:
                backgroundImage.sprite = commonSprite;
                break;
        }
    }
}
