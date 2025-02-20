using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBtnManager : MonoBehaviour
{

    [SerializeField] private GameObject[] UpgradeButtonsList;    
    
    private UpgradeBtn UpgradeBtn;

    //���׷��̵� ��ư���� ���ΰ�ħ
    public void RefreshUpgradeBtn()
    {
        foreach(GameObject btn in UpgradeButtonsList)
        {
            UpgradeBtn = btn.GetComponent<UpgradeBtn>();
            UpgradeBtn.CheckUpgradable();
        }
    } 







}
