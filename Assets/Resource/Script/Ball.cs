using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        // GameManager ������Ʈ�� ã�ų� ���� �Ҵ��� �� ����
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ������Ʈ�� Pin�� ���� ����
        if (other.CompareTag("Pin"))
        {
            Vector2 hitPosition = other.transform.position; // �浹 ��ġ ��������
            //gameManager.IncreaseDamage(hitPosition); // GameManager�� ��ġ ���� �� ������ ����
        }
    }
}
