using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneInitializer : MonoBehaviour
{
    [SerializeField] private GameObject GPGSObj;
    [SerializeField] private GameObject startBtn;
    private DataControl dataControl;

    //해당 오브젝트가 활성화되었을 때 작동

    private void Start()
    {
        //클라우드 서버에서 데이터 불러오기
        dataControl = GPGSObj.GetComponent<DataControl>();
        dataControl.LoadData();
        startBtn.SetActive(true);
    }





}
