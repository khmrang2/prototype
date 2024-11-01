using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        // GameManager 오브젝트를 찾거나 직접 할당할 수 있음
        gameManager = FindObjectOfType<GameManager>();
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트가 Pin일 때만 실행
        if (collision.gameObject.CompareTag("Pin"))
        {
            Debug.Log("Collision detected with Pin!");
        }
    }

}
