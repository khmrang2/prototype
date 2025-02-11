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
        //ü�¹� ������Ʈ
        hpSlider.value = playerHP;

        //ü���� 0 ���ϰ� �ȴٸ�
        if(playerHP <= 0)
        {
            OnDied();
        }
    }


    //��� ó�� �Լ�
    void OnDied()
    {
        Destroy(hpBar);
        Destroy(this.gameObject);
    }
}
