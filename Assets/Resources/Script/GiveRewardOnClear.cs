using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GiveRewardOnClear : MonoBehaviour
{
    public PlayerState playerState;

    [Header("Clear reward settings")]
    [SerializeField] private int clearGold;
    [SerializeField] private TextMeshProUGUI rewardText;




    private void OnEnable()
    {
        clearGold = (int)(clearGold * (1.0f + playerState.Player_More_Economy));
        //게임이 클리어 되어 팝업이 활성화된 경우에 작동
        if (this.gameObject.activeSelf)
        {
            //서버에 데이터 저장 가능 여부에 따라 작동

            //저장 성공시 

            //보상 부여
            int gold = int.Parse(DataControl.LoadEncryptedDataFromPrefs("Gold"));
            DataControl.SaveEncryptedDataToPrefs("Gold", (gold + clearGold).ToString());
            rewardText.text = "보상: " + clearGold + "G";


            //스테이지 클리어 정보 저장
        }

    }
}
