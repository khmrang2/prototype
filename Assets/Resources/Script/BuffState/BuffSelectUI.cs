using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuffSelectUI : MonoBehaviour
{
    private BuffStruct buffstruct;
    private Button button;

    public TextMeshProUGUI buffname;
    public TextMeshProUGUI tooltip;
    public Image bufficon;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void getBuffState(BuffStruct bs)
    {
        buffstruct = bs;
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

    void updateUI()
    {
        buffname.text = buffstruct.Name;
        tooltip.text = buffstruct.Tooltip;
        bufficon.sprite = Resources.Load<Sprite>(buffstruct.ImagePath);
    }
}
