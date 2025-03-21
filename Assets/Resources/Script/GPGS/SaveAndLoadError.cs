using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoadError : MonoBehaviour
{
    [SerializeField] private GameObject ErrorScreen;



    public void ShowErrorScreen()
    {   
        //오류 팝업 띄우기
        ErrorScreen.SetActive(true);
        
    }



    void Start()
    {
        ErrorScreen.SetActive(false);
    }
}
