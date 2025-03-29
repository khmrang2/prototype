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

    public Button yesButton; // yes ��ư
    public Button noButton;  // no ��ư
    // �̹� �� �������� SlotInven.cs�� item�� ����.
    public AudioSource close_button_click_sound;
    public AudioSource equip_button_click_sound;
    public AudioSource unequip_button_click_sound;

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
                itemPrimaryStatName.text = "공격력";
                itemSecondaryStatName.text = "공의 수";
                break;
            case EquipmentType.Heart:
                itemPrimaryStatName.text = "체력";
                itemSecondaryStatName.text = "공격력";
                break;
            case EquipmentType.Gear:
                itemPrimaryStatName.text = "공의 수";
                itemSecondaryStatName.text = "핀 체력";
                break;
            default:
                Debug.LogError("[�������˾�] : ������ ���� ���� ���� �߸� �ҷ���.");
                break;
        }

        if (popupIdentifier == 1)
        {
            // ��� ������ ������ ��
            //Debug.LogError("��� - �����ۿ� ����.");
            yesButton.GetComponentInChildren<TextMeshProUGUI>().text = "해제";
        }
        else if (popupIdentifier == 0)
        {
            // �κ��丮���� ȣ��� ���:
            // ��� Ÿ�Կ� �ش��ϴ� ���Կ� �̹� �����Ǿ� �ִٸ� "��ü", �ƴϸ� "����"
            if (Equip.Instance.IsEquipped(equip.EquipType))
            {
                //Debug.LogError("�κ��丮 - ������ ����");
                yesButton.GetComponentInChildren<TextMeshProUGUI>().text = "교체";

            }
            else
            {
                //Debug.LogError("�κ��丮 - X�� ����");
                yesButton.GetComponentInChildren<TextMeshProUGUI>().text = "장착";
            }
        }
    }

    public void yesClicked()
    {
        if (popupIdentifier == 1)
        {
            PlayOneShotSound(unequip_button_click_sound);
            popUpUI.SetActive(false);
        }
        else if (popupIdentifier == 0)
        {
            PlayOneShotSound(equip_button_click_sound);
            popUpUI.SetActive(false);
        }

        if (popupIdentifier == 1)
        {
            // ��񿡼� ȣ�� �� ��� :
            // �ٷ� ����
            Equip.Instance.UnEquipItem(equip, true);
        }
        else if (popupIdentifier == 0)
        {
            Equipment currentEquipped = Equip.Instance.GetEquippedItem(equip.EquipType);
            // �κ��丮���� ȣ��� ���:
            // ��� Ÿ�Կ� �ش��ϴ� ���Կ� �̹� �����Ǿ� �ִٸ� "��ü", �ƴϸ� "����"
            if (Equip.Instance.IsEquipped(equip.EquipType))
            {
                Equip.Instance.UnEquipItem(currentEquipped, false);
            }

            // ��� ���� ���� ����
            Equip.Instance.EquipItem(equip, amount, true, true);
        }
    }
    public void noClicked()
    {
        if (popUpUI == null) return;

        PlayOneShotSound(close_button_click_sound);
        popUpUI.SetActive(false);
    }
    
    private void PlayOneShotSound(AudioSource source)
    {
        if (source == null || source.clip == null) return;

        // 임시 오브젝트 생성
        GameObject tempAudioObj = new GameObject("TempAudio");
        AudioSource tempAudio = tempAudioObj.AddComponent<AudioSource>();
        tempAudio.clip = source.clip;
        tempAudio.outputAudioMixerGroup = source.outputAudioMixerGroup; // 믹서 연결 유지
        tempAudio.volume = source.volume;
        tempAudio.spatialBlend = 0f; // 2D
        tempAudio.Play();

        Destroy(tempAudioObj, tempAudio.clip.length);
    }
}
