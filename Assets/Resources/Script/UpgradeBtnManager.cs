using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBtnManager : MonoBehaviour
{

    public UpgradeBtn curBtn;                // 현재 활성화 된 버튼

    public void trackingBtn(UpgradeBtn btn)
    {
        Debug.Log("눌렸나?");
        // curBtn이 활성화 된 상태라면.
        if (curBtn != null)
        {
            if (curBtn == btn) return;
            curBtn.hideButtons();
            curBtn = btn;
        }
        else // 최초의 curBtn에 할당이 안된 상태에서 버튼이 눌렸다면.
        {
            curBtn = btn;
        }
    }
}
