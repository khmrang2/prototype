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

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트가 Pin일 때만 실행
        if (other.CompareTag("Pin"))
        {
            Vector2 hitPosition = other.transform.position; // 충돌 위치 가져오기
            //gameManager.IncreaseDamage(hitPosition); // GameManager에 위치 전달 및 데미지 증가
        }
    }
}
