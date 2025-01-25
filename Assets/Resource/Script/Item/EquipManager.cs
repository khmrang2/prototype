using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{

    // 장착된 슬롯 인벤토리 3개
    public GameObject[] equipSlot;

    private int index = 0;

    public void equipItem()
    {
        index++;
    }
    public void unequipItem()
    {

        index--;
    }

    public void test()
    {

    }
}
