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
    public Image bufficon;
    public TextMeshProUGUI tootlip;

    public void getBuffState(BuffStruct bs)
    {
        buffstruct = bs;
        updateUI();
    }

    void updateUI()
    {
        tootlip.text = buffstruct.Tooltip;
        bufficon.sprite = Resources.Load<Sprite>("Image/Items/" + buffstruct.ImagePath);
    }
}
