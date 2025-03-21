using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class updatePopup : MonoBehaviour
{
    [Header("Handling UI for update")]
    public SlotInven slotUI;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemPrimaryStatName;
    public TextMeshProUGUI itemPrimaryStat;
    public TextMeshProUGUI itemSecondaryStatName;
    public TextMeshProUGUI itemSecondaryStat;

    public FontSizeAdjuster fontSizeAdjuster;

    public GameObject popUpUI; // �˾� ui

    public Button yesButton; // yes ��blic 
    public AudioSource equipSound;
    public AudioSource unequipSound;
    public Button noButton;  // no ��ư
    // �̹� �� �������� SlotInven.cs�� item�� ����.
    private Equipment equip;
    private int amount;
    private int popupIdentifier; // ���(0)���� �°��� ����(1)���� �°��� 

    // ���� ������ ���̵�� ������ �����ɴϴ�. 
    public void loadItem(int popupid, ItemData itemData)
    {
        popupIdentifier = popupid;
        equip = (Equipment)itemData.item;
        amount = itemData.amount;

        slotUI.setInit(itemData);
        loadData(equip);
        updateWithRairity(equip.Rarity);
        fontSizeAdjuster.adjustFont();
    }

    /// <summary>
    /// 
    /// ������ �̸��� ���Ƽ�� ���� ���� �����Ѵ�.
    /// </summary>
    /// <param name="rarity"></param>
    private void updateWithRairity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                itemName.color = new Color(0.5f, 0.5f, 0.5f);
                break;
            case Rarity.Uncommon:
                itemName.color = new Color(0.2f, 0.8f, 0.3f);
                break;
            case Rarity.Rare:
                itemName.color = new Color(0.25f, 0.4f, 1f);
                break;
            case Rarity.Epic:
                itemName.color = new Color(1f, 0.78f, 0.25f);
                break;
            case Rarity.Legendary:
                itemName.color = new Color(1f, 0.3f, 0.3f);
                break;
            default:
                Debug.LogError("[�������˾�] : ������ ���Ƽ �߸� �ҷ���.");
                break;
        }
    }

    private void loadData(Equipment equip)
    {
        itemName.text = equip.ItemName;
        itemDescription.text = equip.Tooltip;
        itemPrimaryStat.text = equip.MainStatValue.ToString();
        itemSecondaryStat.text = equip.SubStatValue.ToString();
        // �������ڵ���������.. itemdata.json ���� ������ �ʿ���.
        switch (equip.EquipType)
        {
            case EquipmentType.Weapon:
                itemPrimaryStatName.text = "���ݷ�";
                itemPrimaryStatName.text = "�� ����";
                break;
            case EquipmentType.Heart:
                itemPrimaryStatName.text = "ü��";
                itemSecondaryStatName.text = "���ݷ�";
                break;
            case EquipmentType.Gear:
                itemPrimaryStatName.text = "�� ü��";
                itemSecondaryStatName.text = "�� ����";
                break;
            default:
                Debug.LogError("[�������˾�] : ������ ���� ���� ���� �߸� �ҷ���.");
                break;
        }

        if (popupIdentifier == 1)
        {
            // ��� ������ ������ ��
            //Debug.LogError("��� - �����ۿ� ����.");
            yesButton.GetComponentInChildren<TextMeshProUGUI>().text = "����";
        }
        else if (popupIdentifier == 0)
        {
            // �κ��丮���� ȣ��� ���:
            // ��� Ÿ�Կ� �ش��ϴ� ���Կ� �̹� �����Ǿ� �ִٸ� "��ü", �ƴϸ� "����"
            if (Equip.Instance.IsEquipped(equip.EquipType))
            {
                //Debug.LogError("�κ��丮 - ������ ����");
                yesButton.GetComponentInChildren<TextMeshProUGUI>().text = "��ü";

            }
            else
            {
                //Debug.LogError("�κ��丮 - X�� ����");
                yesButton.GetComponentInChildren<TextMeshProUGUI>().text = "����";
            }
        }
    }

    public void yesClicked()
    {
        if(popupIdentifier == 1) unequipSound.Play();
        else equipSound.Play();
        StartCoroutine(DisablePopupAfterSound());
        
        if (popupIdentifier == 1)
        {
            // ��񿡼� ȣ�� �� ��� :
            // �ٷ� ����
            Equip.Instance.UnEquipItem(equip, true);
        }
        else if(popupIdentifier == 0)
        {
            Equipment currentEquipped = Equip.Instance.GetEquippedItem(equip.EquipType);
            // �κ��丮���� ȣ��� ���:
            // ��� Ÿ�Կ� �ش��ϴ� ���Կ� �̹� �����Ǿ� �ִٸ� "��ü", �ƴϸ� "����"
            if (Equip.Instance.IsEquipped(equip.EquipType))
            {
                Equip.Instance.UnEquipItem(currentEquipped, false);
            }
            // ��� ���� ���� ����
            Equip.Instance.EquipItem(equip, amount,true, true);
        }
    }

    public void noClicked()
    {
        if (popUpUI == null) return;
        // �˾��� ��� ��Ȱ��ȭ
        popUpUI.SetActive(false);
    }
    
    IEnumerator DisablePopupAfterSound()
    {
        yield return new WaitForSeconds(0.3f);
        popUpUI.SetActive(false);
    }
}
