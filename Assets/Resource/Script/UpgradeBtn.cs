using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBtn : MonoBehaviour
{
    public Button btn;    // ���� ��ư
    public UpgradeBtnManager manager;    // ��ư �Ŵ��� // ��ȣ�� �ֱ� ���ؼ�
    public GameObject yesButton; // �����տ� �����ϴ� yes��ư
    public GameObject noButton; // �̸� �غ�� ��ư 2

    void Start()
    {
        // �ʱ� ���¿��� �� ��ư ��Ȱ��ȭ
        yesButton.SetActive(false);
        noButton.SetActive(false);

        // ���� ��ư�� Ŭ�� �̺�Ʈ �߰�
        btn.onClick.AddListener(showButtons);
    }

    public void showButtons()
    {
        // �� ��ư Ȱ��ȭ
        yesButton.SetActive(true);
        noButton.SetActive(true);
        // ��ư Ʈ��ŷ.
        manager.trackingBtn(this);
    }

    public void hideButtons()
    {
        // �� ��ư Ȱ��ȭ
        yesButton.SetActive(false);
        noButton.SetActive(false);
    }
}
