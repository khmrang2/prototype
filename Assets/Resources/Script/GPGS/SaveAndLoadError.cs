using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoadError : MonoBehaviour
{
    private static GameObject ErrorScreen;



    public static void ShowErrorScreen()
    {   
        //¿À·ù ÆË¾÷ ¶ç¿ì±â
        ErrorScreen.SetActive(true);
        
    }



    void Start()
    {
        ErrorScreen = GameObject.FindWithTag("ErrorScreen");
        ErrorScreen.SetActive(false);
    }





   
}
