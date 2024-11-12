using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyListManager : MonoBehaviour
{
    // �� �������� ���� ���� (Unity �����Ϳ��� �Ҵ�)
    public GameObject enemyPrefab;
    public Transform playerTransform;
    public Transform enemyTransform; // ���Ͱ� �����Ǵ� ���� ��ġ
    private List<Enemy> enemies = new List<Enemy>();
    public int maxEnemies = 5; // �ִ� 5����
    public float spawnInterval = 5f; // 5�� �������� ���� ����
    void Start()
    {
        // ���͸� �����ϴ� �ڷ�ƾ ����
        StartCoroutine(SpawnEnemiesWithInterval());
    }
    public IEnumerator SpawnEnemiesWithInterval()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            // ���� �����ϰ� ����Ʈ�� �߰�
            GameObject enemyObject = Instantiate(enemyPrefab, enemyTransform.position, Quaternion.identity);
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            enemies.Add(enemy);

            // 5�� ��� �� ���� �� ����
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    // ��� ���� �ൿ�� ó���ϴ� �޼���
    public void HandleEnemyBehavior()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.Act(playerTransform); // �� ���� �÷��̾ ���� �̵��ϵ��� ��
            }
        }
    }
}
