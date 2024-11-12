using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ball : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        // GameManager ������Ʈ�� ã�ų� ���� �Ҵ��� �� ����
        gameManager = FindObjectOfType<GameManager>();
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� ������Ʈ�� Pin�� ���� ����
        if (collision.gameObject.CompareTag("Pin"))
        {
            Debug.Log("Collision detected with Pin!");
        }
    }

}
