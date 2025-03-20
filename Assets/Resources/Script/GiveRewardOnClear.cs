using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveRewardOnClear : MonoBehaviour
{
    public PlayerState playerState;

    [Header("Clear reward settings")]
    [SerializeField] private int clearGold;


    private void OnEnable()
    {
        clearGold = (int)(clearGold * playerState.Player_More_Economy);
        //������ Ŭ���� �Ǿ� �˾��� Ȱ��ȭ�� ��쿡 �۵�
        if (this.gameObject.activeSelf)
        {
            //������ ������ ���� ���� ���ο� ���� �۵�

            //���� ������ 

            //���� �ο�
            int gold = int.Parse(DataControl.LoadEncryptedDataFromPrefs("Gold"));
            DataControl.SaveEncryptedDataToPrefs("Gold", (gold + clearGold).ToString());


            //�������� Ŭ���� ���� ����
        }

    }
}
