using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotInven : MonoBehaviour
{
    [SerializeField] public string itemID = "1"; // ������ �ĺ���

    [SerializeField] public ItemDataLoader itemDataLoader;

    public string getItemID()
    {
        return itemID;
    }
}
