using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class BuffSelectUI : MonoBehaviour
{
    private BuffStruct buffstruct;
    private Button button;

    public TextMeshProUGUI buffname;
    public TextMeshProUGUI tooltip;
    public Image bufficon;
    [SerializeField] private GameObject _AdLockMask;

    [SerializeField] private Color NormalBuff;
    [SerializeField] private Color EpicBuff;


    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void getBuffState(BuffStruct bs, bool isAd)
    {
        buffstruct = bs;
        adLock(isAd);
        updateUI();
    }

    public void RegisterBuffSelectionCallback(System.Action<int> callback)
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => callback(buffstruct.ID));
        }
    }

    public void UnlockAdBuff()
    {
        adLock(false);
    }

    void updateUI()
    {
        buffname.text = buffstruct.Name;
        tooltip.text = buffstruct.Tooltip;
        bufficon.sprite = Resources.Load<Sprite>(buffstruct.ImagePath);

        if (this.buffstruct.Rank == BuffRank.Normal)
        {
            buffname.color = NormalBuff;
            tooltip.color = NormalBuff;
        }
        else if (this.buffstruct.Rank == BuffRank.Epic)
        {
            buffname.color = EpicBuff;
            tooltip.color = EpicBuff;
        }
        else
        {
            Debug.LogError("buff ui update failed!");
        }
    }
    private void adLock(bool isAd)
    {
        if (_AdLockMask != null)
        {
            _AdLockMask.SetActive(isAd);
        }
    }
}
