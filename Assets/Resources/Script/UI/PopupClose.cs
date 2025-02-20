using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupClose : MonoBehaviour
{
    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }
}
