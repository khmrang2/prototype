using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{

    // ������ ���� �κ��丮 3��
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
