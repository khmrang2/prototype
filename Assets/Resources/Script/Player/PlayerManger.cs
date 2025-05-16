using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerManger : MonoBehaviour
{
    // 체력 비율 관련 delegate 정의
    public delegate void HealthRatioChangedHandler(float healthRatio);
    public static event HealthRatioChangedHandler OnHealthRatioChanged;
    
    // 체력 임계값 (50%)
    [SerializeField] private float healthThreshold = 0.5f;
    private bool isLowHealth = false;

    [Header("Player Damage")]
    public PlayerAnimatorMobile animator;
    public ProjectileOnHit plAtkObj;    //플레이어가 공격 시 발사하는 투사체
    private Color AtkObjColor;

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
    public RectTransform playerSpawnTransform;

    [Header("Player Stat Script")]
    public PlayerState playerState;
    public PlayerStatus playerStatus;


    [Header("GameOver Screen")]
    public GameObject GameOverPopup;

    [Header("버프들 적용을 위한 불러올 것들")]
    public GameManager gameManager;
    public PinManager pinManager;

    private bool isAtking = false;

    private float prevMaxHP;

    void Start()
    {
        //변수 및 팝업 초기화
        gameOver = false ;
        GameOverPopup.SetActive(false);
        playerHP = playerStatus.PlayerHP;
        maxHP = playerStatus.PlayerHP;
        prevMaxHP = maxHP;

        this.gameObject.transform.position = playerSpawnTransform.position;
        //체력바 소환
        //hpBar = Instantiate(prefHP_Bar, canvas.transform);
        
        //체력바 위치 조정
        //Vector3 viewportPos = Camera.main.WorldToViewportPoint(this.gameObject.transform.position + Vector3.up * height + Vector3.left * 0.07f);   //체력바가 위치할 좌표
        //RectTransform rt = hpBar.GetComponent<RectTransform>();
        //rt.anchorMin = viewportPos;
        //rt.anchorMax = viewportPos;
        hpSlider = hpBar.GetComponent<Slider>();
        hpSlider.maxValue = maxHP;
        hpSlider.value = playerHP;
        
        // 초기 체력 비율 이벤트 발생
        CheckAndNotifyHealthRatio();
    }

    // Update is called once per frame
    void Update()
    {
        // maxHP를 PlayerState에서 받아옴
        float newMaxHP = playerState.Player_Health;
        if (Mathf.Abs(newMaxHP - prevMaxHP) > 0.01f)
        {
            float delta = newMaxHP - prevMaxHP;
            maxHP = newMaxHP;
            if (delta > 0)
            {
                playerHP += delta;
            }
            else if (delta < 0)
            {
                playerHP = Mathf.Min(playerHP, maxHP);
            }
            prevMaxHP = maxHP;
            
            // 체력 변경 시 체력 비율 체크
            CheckAndNotifyHealthRatio();
        }

        hpSlider.maxValue = maxHP;
        hpSlider.value = Mathf.Min(playerHP, maxHP);

        //체력이 0 이하가 된다면
        if (playerHP <= 0 && !gameOver)
        {
            OnDied();
        }
    }
    
    // 체력 비율을 체크하고 변화가 있으면 이벤트 발생
    private void CheckAndNotifyHealthRatio()
    {
        if (maxHP <= 0) return;
        
        float currentRatio = playerHP / maxHP;
        bool currentLowHealth = currentRatio <= healthThreshold;
        
        // 체력 비율이 임계값 이하로 내려갔거나 다시 올라갔을 때만 이벤트 발생
        if (currentLowHealth != isLowHealth)
        {
            isLowHealth = currentLowHealth;
            OnHealthRatioChanged?.Invoke(currentRatio);
            Debug.Log($"체력 비율 변화: {currentRatio:F2}, 낮은 체력 상태: {isLowHealth}");
        }
    }

    // 현재 체력 비율을 반환하는 메서드
    public float GetCurrentHealthRatio()
    {
        return maxHP > 0 ? playerHP / maxHP : 1f;
    }
    
    // 현재 낮은 체력 상태인지 반환하는 메서드
    public bool IsLowHealth()
    {
        return isLowHealth;
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

    public void getHitted(int damage)
    {
        Debug.Log("플레이어가 데미지를 얼마얼마 얻음.");
        animator.TriggerDamage();
        playerHP -= damage;
        
        // 체력 변경 시 체력 비율 체크
        CheckAndNotifyHealthRatio();
        
        Debug.Log("getHitted탈출..");
    }

    //핀 히트 수와 공격력의 곱인 총 데미지를 계산 후 반환하는 함수, 플레이어가 발사하는 투사체에서 호출
    public int GetTotalDamage()
    {
        // 크리티컬 발생 시에
        if (playerState.Player_Critical_Chance >= UnityEngine.Random.Range(0f, 1f))
        {
            //Debug.LogError("크리티컬!");
            //Debug.LogError($"부여 {(int)((playerState.Player_Damage) * (gameManager.pinHitCount) * playerState.Player_Critical_Damage)} <- {(playerState.Player_Damage)} * {(gameManager.pinHitCount)}");
            return (int)((playerState.Player_Damage) * (gameManager.pinHitCount) * playerState.Player_Critical_Damage);
        }
        else
        {
            return (playerState.Player_Damage) * (gameManager.pinHitCount);
        }
    }

    public void attackAnim()
    {
        isAtking = true;
        animator.TriggerAttack();
    }

    public void OnAttackAnimEnd()
    {
        isAtking = false;
    }

    public void attackObj()
    {
        plAtkObj.StartAttack();
    }

    public bool doneAtk()
    {
        return !isAtking;
    }
}
