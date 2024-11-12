using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManger : MonoBehaviour
{
    [Header ("HP Bar variables")]
    public GameObject prefHP_Bar;
    public GameObject canvas;
    public float height = 2f;
    private GameObject hpBar;
    private Slider hpSlider; 

    [Header("Player Chracter Variables")]
    public float playerHP = 3;


    void Start()
    {
        hpBar = Instantiate(prefHP_Bar, canvas.transform);
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(this.gameObject.transform.position + Vector3.up * height);
        RectTransform rt = hpBar.GetComponent<RectTransform>();
        rt.anchorMin = viewportPos;
        rt.anchorMax = viewportPos;
         hpSlider = hpBar.GetComponent<Slider>();

    }

    // Update is called once per frame
    void Update()
    {
        //체력바 업데이트
        hpSlider.value = playerHP;

        //체력이 0 이하가 된다면
        if(playerHP <= 0)
        {
            OnDied();
        }
    }


    //사망 처리 함수
    void OnDied()
    {
        Destroy(hpBar);
        Destroy(this.gameObject);
    }
}
