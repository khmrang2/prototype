using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneInitializer : MonoBehaviour
{
    [SerializeField] private GameObject GPGSObj;
    [SerializeField] private GameObject startBtn;
    private DataControl dataControl;

    //�ش� ������Ʈ�� Ȱ��ȭ�Ǿ��� �� �۵�

    private void Start()
    {
        //Ŭ���� �������� ������ �ҷ�����
        dataControl = GPGSObj.GetComponent<DataControl>();
        dataControl.LoadData();
        startBtn.SetActive(true);
    }





}
