using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBtnManager : MonoBehaviour
{

    [SerializeField] private GameObject[] UpgradeButtonsList;    
    
    private UpgradeBtn UpgradeBtn;

    //업그레이드 버튼들의 새로고침
    public void RefreshUpgradeBtn()
    {
        foreach(GameObject btn in UpgradeButtonsList)
        {
            UpgradeBtn = btn.GetComponent<UpgradeBtn>();
            UpgradeBtn.CheckUpgradable();
        }
    } 







}
