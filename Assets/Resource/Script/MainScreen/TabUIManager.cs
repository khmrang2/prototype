using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class TabUIManager : MonoBehaviour
{
    // ��ũ�� �迭
    [SerializeField] private GameObject[] screens;
    // �� �迭
    [SerializeField] private GameObject[] tabButtons;


    // ���� ����ÿ� screens �迭 �޸� �Ҵ�.
    // ���� ȭ�� ������ �÷��� ȭ������.
    private void Start()
    {
        ShowScreen(2);
    }

    // ShowScreen(int btnId)
    //  : �� ��ư���� �Է��� �����Ͽ� �ش� ��ư�� �Ҵ�� ȭ������ ��ȯ�Ѵ�.
    //  : �����Ǹ�, ��ư�� ������ ��ġ(activePositionOffset), ����� �� �ٲ��ش�.
    // @param : int btnId : btnId
    //  : �� �� ��ư���� invoke ���ִ� index id �̴�.
    public void ShowScreen(int btnId)
    {
        // ���� ó��.
        if(btnId < 0 || btnId > 5)
        {
            Debug.LogWarning("[TabUIManager] : Wrong btnId" + btnId);
            return;
        }

        // btnId�� invoke�� ȭ���� ������ ȭ��� ��Ȱ��ȭ.
        for(int i = 0; i < tabButtons.Length; i++)
        {
            tabButtons[i].GetComponent<TabButton>().Deactivate();
            screens[i].SetActive(false);
        }
        // btnId ȭ�� Ȱ��ȭ.
        screens[btnId].SetActive(true);
        tabButtons[btnId].GetComponent<TabButton>().Activate();
    }
}

