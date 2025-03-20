using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoadError : MonoBehaviour
{
    private static GameObject ErrorScreen;



    public static void ShowErrorScreen()
    {   
        //오류 팝업 띄우기
        ErrorScreen.SetActive(true);
        
    }



    void Start()
    {
        ErrorScreen = GameObject.FindWithTag("ErrorScreen");
        ErrorScreen.SetActive(false);
    }
}
