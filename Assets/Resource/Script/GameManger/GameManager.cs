using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct buffState
{
    int numberOfBalls;
    //int spawnOffset;
    int damageOfBall;
};

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab; // ����� �� ������
    public int numberOfBalls = 5; // ����߸� ���� ����
    public float spawnOffset = 0.1f; // ���� ���ݾ� �ٸ� ��ġ�� �����ǵ��� �ϱ� ���� ������

    private int damage = 0;

    void Update()
    {
        // ȭ���� ��ġ�ߴ��� Ȯ��
        /*
         * �÷��̾ ȭ���� ��ġ�� �κ��� ��ǥ�� �޽��ϴ�.
         * �ش� ��ġ�� ���� ���� ��ŭ Instantiate�� ���� ��ġ�� ball prefab�� ��ȯ�մϴ�..
         * ball �����տ� ���� ���� �̹� ����� ��������(Resource/Prefabs�� ��ġ��. ���� ���͸���, ������ٵ� �� �� ����Ǿ�����)
         */
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPosition.z = 0; // Z���� 0���� �����Ͽ� 2D ��鿡 ���� �����ǵ��� ��

            // ������ ������ ���� ����߸���
            for (int i = 0; i < numberOfBalls; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-spawnOffset, spawnOffset), Random.Range(-spawnOffset, spawnOffset), 0);
                Instantiate(ballPrefab, spawnPosition + offset, Quaternion.identity);
            }
        }
    }

    //void (Vector3 textPrintPoint)
    //{

    //}
}
