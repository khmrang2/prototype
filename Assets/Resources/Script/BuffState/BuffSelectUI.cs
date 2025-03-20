using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuffSelectUI : MonoBehaviour
{
    private BuffStruct buffstruct;

    public TextMeshProUGUI buffname;
    public TextMeshProUGUI tooltip;
    public Image bufficon;

    public void getBuffState(BuffStruct bs)
    {
        buffstruct = bs;
        updateUI();
    }

    void updateUI()
    {
        buffname.text = buffstruct.Name;
        tooltip.text = buffstruct.Tooltip;
        bufficon.sprite = Resources.Load<Sprite>(buffstruct.ImagePath);
    }
}
