using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotInven : MonoBehaviour
{
    public ItemData itemData;
    public int id = 0;

    [Header("Popup Comoponent")]
    public GameObject popUpPanel; // Ȱ��ȭ�� ������
    public updatePopup popUpScript;

    public Image backgroundImage;

    [Header("Set Rairity")]
    public Sprite commonSprite;    // rarity 0
    public Sprite uncommonSprite;  // rarity 1
    public Sprite rareSprite;      // rarity 2
    public Sprite epicSprite;      // rarity 3
    public Sprite legendarySprite; // rarity 4
    public Sprite usableSprite; // rarity 5

    [Header("Set Item Image.")]
    public GameObject inventoryItemPrefab;  // ������ ������ ������.
    private GameObject showingItemObject;


    /// <summary>
    /// �˾��� ����.
    /// </summary>
    public void showPopup()
    {
        if (itemData.item.Rarity != Rarity.Usuable)
        {
            PopUpManager.Instance.ShowPopup(id, itemData);
        }
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
            case Rarity.Usuable:
                backgroundImage.sprite = usableSprite;
                break;
            default:
                backgroundImage.sprite = commonSprite;
                break;
        }
    }

    public void ClearSlot()
    {
        // ������ �����͸� ����
        itemData = null;

        // ��� �̹����� �⺻ ��������Ʈ�� ���� (���⼭�� commonSprite�� �⺻������ ���)
        if (backgroundImage != null)
        {
            backgroundImage.sprite = commonSprite;
        }

        // ������ �ڽ� ������Ʈ�� (��: ������ �̹���, ���� �ؽ�Ʈ ��)�� �⺻ ���·� �ʱ�ȭ
        // ���⼭�� �ܼ��� Ȱ��ȭ�� ���� �� ����ó�� ���̰� �մϴ�.
        foreach (Transform child in transform)
        {
            // ���� �� �������� �߰��Ǹ�, �ش� ������Ʈ���� �ٽ� Ȱ��ȭ�ϰ� ������Ʈ�� ���Դϴ�.
            child.gameObject.SetActive(false);
        }
    }

    public void setInit(ItemData itemdata)
    {
        this.itemData = itemdata;

        setItem(itemdata);
        SetRarity(itemdata.item.Rarity);

        foreach (Transform child in transform)
        {
            // ���� �� �������� �߰��Ǹ�, �ش� ������Ʈ���� �ٽ� Ȱ��ȭ�ϰ� ������Ʈ�� ���Դϴ�.
            child.gameObject.SetActive(true);
        }
    }

    private void setItem(ItemData itemdata)
    {
        Item item = itemdata.item;
        int a = itemdata.amount;


        // ���׾����� ó�� ȣ��Ǹ� �ν��Ͻ� ����
        if (showingItemObject == null)
        {
            // false �ɼ����� Instantiate�ϸ�, �������� ���� RectTransform ����(��Ŀ, �ǹ� ��)�� �����˴ϴ�.
            showingItemObject = Instantiate(inventoryItemPrefab, this.transform, false);
        }
        else
        {
            // �̹� �����ϸ� Ȱ��ȭ
            showingItemObject.GetComponent<Image>().sprite = item.Sprite;
        }
        showingItemObject.transform.SetParent(this.transform, false);

        RectTransform rect = showingItemObject.GetComponent<RectTransform>();
        if (rect != null)
            rect.anchoredPosition = Vector2.zero;

        Image itemImage = showingItemObject.GetComponent<Image>();
        itemImage.sprite = item.Sprite;
        showingItemObject.name = item.ItemName;

        // �ڽ� �ؽ�Ʈ�� ���� ǥ�� (���� ǥ�ÿ� TextMeshProUGUI�� inventoryItemPrefab�� ù ��° �ڽ��̶�� ����)
        TextMeshProUGUI qtyText = showingItemObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        qtyText.text = a.ToString();
    }
}
