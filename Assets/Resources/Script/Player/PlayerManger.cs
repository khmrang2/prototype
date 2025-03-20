using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManger : MonoBehaviour
{
    [Header ("HP Bar variables")]
    public GameObject hpBar;    //�÷��̾��� ü�¹� ui
    //public GameObject canvas;   
    //public float height = 2f;   
    
    private Slider hpSlider; 

    [Header("Player Chracter Variables")]
    public float playerHP = 3;
    public bool isAlive = true;
    public bool gameOver = false;

    [Header("Player Stat Script")]
    public PlayerState playerState;
    public PlayerStatus playerStatus;


    [Header("GameOver Screen")]
    public GameObject GameOverPopup;

    [Header("������ ������ ���� �ҷ��� �͵�")]
    public GameManager gameManager;
    public PinManager pinManager;

    void Start()
    {
        //���� �� �˾� �ʱ�ȭ
        gameOver = false ;
        GameOverPopup.SetActive(false);
        playerHP = playerStatus.PlayerHP;

        //ü�¹� ��ȯ
        //hpBar = Instantiate(prefHP_Bar, canvas.transform);
        
        //ü�¹� ��ġ ����
        //Vector3 viewportPos = Camera.main.WorldToViewportPoint(this.gameObject.transform.position + Vector3.up * height + Vector3.left * 0.07f);   //ü�¹ٰ� ��ġ�� ��ǥ
        //RectTransform rt = hpBar.GetComponent<RectTransform>();
        //rt.anchorMin = viewportPos;
        //rt.anchorMax = viewportPos;
        hpSlider = hpBar.GetComponent<Slider>();
        //hpSlider.maxValue = int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_HP"));
        hpSlider.maxValue = playerStatus.PlayerHP;

    }

    // Update is called once per frame
    void Update()
    {
        //ü�¹� ������Ʈ
        hpSlider.value = playerHP;
        hpSlider.maxValue = playerState.Player_Health;
        //ü���� 0 ���ϰ� �ȴٸ�
        if (playerHP <= 0 && !gameOver)
        {
            OnDied();
        }
    }


    //��� ó�� �Լ�
    void OnDied()
    {
        gameOver = true ;
        isAlive = false;
        Destroy(hpBar);

        //��� ���ϸ��̼�
        this.gameObject.SetActive(false);

        //���ϸ��̼� ���� �� ���� �Ͻ� ����
        //Time.timeScale = 0;

        //���� ���� �˾� ����
        GameOverPopup.SetActive(true);

    }

    //�� ��Ʈ ���� ���ݷ��� ���� �� �������� ��� �� ��ȯ�ϴ� �Լ�, �÷��̾ �߻��ϴ� ����ü���� ȣ��
    public int GetTotalDamage()
    {
        // ũ��Ƽ�� �߻� �ÿ�
        if (playerState.Player_Critical_Chance >= Random.Range(0, 100))
        {
            //Debug.LogError("ũ��Ƽ��!");
            //Debug.LogError($"{(int)((playerState.Player_Damage) * (gameManager.pinHitCount) * playerState.Player_Critical_Damage)} <- {(playerState.Player_Damage) * (gameManager.pinHitCount)}");
            return (int)((playerState.Player_Damage) * (gameManager.pinHitCount) * playerState.Player_Critical_Damage);
        }
        else
        {
            return (playerState.Player_Damage) * (gameManager.pinHitCount);
        }
    }
}
