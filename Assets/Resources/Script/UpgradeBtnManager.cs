using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBtnManager : MonoBehaviour
{

    public UpgradeBtn curBtn;                // ���� Ȱ��ȭ �� ��ư

    public void trackingBtn(UpgradeBtn btn)
    {
        Debug.Log("���ȳ�?");
        // curBtn�� Ȱ��ȭ �� ���¶��.
        if (curBtn != null)
        {
            if (curBtn == btn) return;
            curBtn.hideButtons();
            curBtn = btn;
        }
        else // ������ curBtn�� �Ҵ��� �ȵ� ���¿��� ��ư�� ���ȴٸ�.
        {
            curBtn = btn;
        }
    }
}
