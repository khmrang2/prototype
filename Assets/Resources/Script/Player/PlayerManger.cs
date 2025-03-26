using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManger : MonoBehaviour
{
    [Header("Player Damage")]
    public PlayerAnimatorMobile animator;

    [Header ("HP Bar variables")]
    public GameObject hpBar;    //플레이어의 체력바 ui
    //public GameObject canvas;   
    //public float height = 2f;   
    
    private Slider hpSlider; 

    [Header("Player Chracter Variables")]
    public float playerHP = 3;
    public bool isAlive = true;
    public bool gameOver = false;
    public float maxHP = 3;

    [Header("Player Stat Script")]
    public PlayerState playerState;
    public PlayerStatus playerStatus;


    [Header("GameOver Screen")]
    public GameObject GameOverPopup;

    [Header("버프들 적용을 위한 불러올 것들")]
    public GameManager gameManager;
    public PinManager pinManager;

    private bool isAtking = false;

    void Start()
    {
        //변수 및 팝업 초기화
        gameOver = false ;
        GameOverPopup.SetActive(false);
        playerHP = playerStatus.PlayerHP;
        maxHP = playerStatus.PlayerHP;
        //체력바 소환
        //hpBar = Instantiate(prefHP_Bar, canvas.transform);
        
        //체력바 위치 조정
        //Vector3 viewportPos = Camera.main.WorldToViewportPoint(this.gameObject.transform.position + Vector3.up * height + Vector3.left * 0.07f);   //체력바가 위치할 좌표
        //RectTransform rt = hpBar.GetComponent<RectTransform>();
        //rt.anchorMin = viewportPos;
        //rt.anchorMax = viewportPos;
        hpSlider = hpBar.GetComponent<Slider>();
        //hpSlider.maxValue = int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_HP"));
        hpSlider.maxValue = maxHP;
        hpSlider.value = playerHP;
    }

    // Update is called once per frame
    void Update()
    {
        //체력바 업데이트(버프 적용)
        maxHP = playerState.Player_Health;
        // hp 업데이트.
        hpSlider.maxValue = maxHP;
        hpSlider.value = playerHP;

        //체력이 0 이하가 된다면
        if (playerHP <= 0 && !gameOver)
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

    //핀 히트 수와 공격력의 곱인 총 데미지를 계산 후 반환하는 함수, 플레이어가 발사하는 투사체에서 호출
    public int GetTotalDamage()
    {
        // 크리티컬 발생 시에
        if (playerState.Player_Critical_Chance >= Random.Range(0, 100))
        {
            //Debug.LogError("크리티컬!");
            //Debug.LogError($"{(int)((playerState.Player_Damage) * (gameManager.pinHitCount) * playerState.Player_Critical_Damage)} <- {(playerState.Player_Damage) * (gameManager.pinHitCount)}");
            return (int)((playerState.Player_Damage) * (gameManager.pinHitCount) * playerState.Player_Critical_Damage);
        }
        else
        {
            return (playerState.Player_Damage) * (gameManager.pinHitCount);
        }
    }

    public void getHitted(int damage)
    {
        Debug.Log("플레이어가 데미지를 얼마얼마 얻음.");
        animator.TriggerDamage();
        playerHP -= damage;
        Debug.Log("getHitted탈출..");
    }

    public async Task attackAnim()
    {
        isAtking = true;
        animator.TriggerAttack();
        await Task.Delay(1000);
        isAtking = false;
    }

    public bool doneAtk()
    {
        return !isAtking;
    }
}
