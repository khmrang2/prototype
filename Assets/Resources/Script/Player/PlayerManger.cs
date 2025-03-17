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

    //�÷��̾� ���� ������ ��ũ��Ʈ, (Unity �����Ϳ��� �Ҵ�)
    [Header("Player Status Script")]
    public PlayerStatus playerStatus;


    [Header("GameOver Screen")]
    public GameObject GameOverPopup;

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
        hpSlider.maxValue = int.Parse(DataControl.LoadEncryptedDataFromPrefs("PlayerCharacter_HP"));

    }

    // Update is called once per frame
    void Update()
    {
        //ü�¹� ������Ʈ
        hpSlider.value = playerHP;

        //ü���� 0 ���ϰ� �ȴٸ�
        if(playerHP <= 0 && !gameOver)
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
}
