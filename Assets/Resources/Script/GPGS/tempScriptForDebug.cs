using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class tempScriptForDebug : MonoBehaviour
{

    public TextMeshProUGUI goldText;


    private void Start()
    {
        goldText.text = "0";
    }

    private void Update()
    {
        if (PlayerPrefs.HasKey("Gold"))
        {
            goldText.text = DataControl.LoadEncryptedDataFromPrefs("Gold");
        }
        else
        {
            DataControl.SaveEncryptedDataToPrefs("Gold", goldText.text);
            goldText.text = DataControl.LoadEncryptedDataFromPrefs("Gold");
        }
    }



    public void AddGold()
    {
        int tempGold = int.Parse(goldText.text) + 100;
        DataControl.SaveEncryptedDataToPrefs("Gold", tempGold.ToString());
    }


    public void MinusGold()
    {
        int tempGold = int.Parse(goldText.text) - 100;
        DataControl.SaveEncryptedDataToPrefs("Gold", tempGold.ToString());
    }


}
