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
}
