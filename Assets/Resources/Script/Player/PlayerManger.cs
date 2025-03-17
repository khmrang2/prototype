using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManger : MonoBehaviour
{
    [Header ("HP Bar variables")]
    public GameObject hpBar;    //플레이어의 체력바 ui
    //public GameObject canvas;   
    //public float height = 2f;   
    
    private Slider hpSlider; 

    [Header("Player Chracter Variables")]
    public float playerHP = 3;
    public bool isAlive = true;
    public bool gameOver = false;

    //플레이어 스탯 참조용 스크립트, (Unity 에디터에서 할당)
    [Header("Player Status Script")]
    public PlayerStatus playerStatus;


    [Header("GameOver Screen")]
    public GameObject GameOverPopup;

    void Start()
    {
        //변수 및 팝업 초기화
        gameOver = false ;
        GameOverPopup.SetActive(false);
        playerHP = playerStatus.PlayerHP;

        //체력바 소환
        //hpBar = Instantiate(prefHP_Bar, canvas.transform);
        
        //체력바 위치 조정
        //Vector3 viewportPos = Camera.main.WorldToViewportPoint(this.gameObject.transform.position + Vector3.up * height + Vector3.left * 0.07f);   //체력바가 위치할 좌표
        //RectTransform rt = hpBar.GetComponent<RectTransform>();
        //rt.anchorMin = viewportPos;
        //rt.anchorMax = viewportPos;
        hpSlider = hpBar.GetComponent<Slider>();
        hpSlider.maxValue = int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_HP"));

    }

    // Update is called once per frame
    void Update()
    {
        //체력바 업데이트
        hpSlider.value = playerHP;

        //체력이 0 이하가 된다면
        if(playerHP <= 0 && !gameOver)
        {
            OnDied();
        }
    }


    //사망 처리 함수
    void OnDied()
    {
        gameOver = true ;
        isAlive = false;
        Destroy(hpBar);

        //사망 에니메이션
        this.gameObject.SetActive(false);

        //에니메이션 종료 후 게임 일시 정지
        //Time.timeScale = 0;

        //게임 오버 팝업 띄우기
        GameOverPopup.SetActive(true);

    }
}
