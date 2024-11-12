using System.Collections.Generic;
using UnityEngine;

public class EnemyListManager : MonoBehaviour
{
    // ������ �����ϴ� ����Ʈ
    public GameObject enemyPrefab; // ������ ���� ������
    public List<Enemy> enemies = new List<Enemy>();

    // ������ �ൿ�� �����ϴ� �޼���
    public void ExecuteEnemyBehavior(Transform player)
    {
// enemies ����Ʈ�� player�� null���� Ȯ��
        if (enemies == null || player == null)
        {
            Debug.LogError("Enemies list or player reference is not assigned!");
            return;
        }

        foreach (var enemy in enemies)
        {
            if (enemy != null) // ���� null�� �ƴ��� Ȯ��
            {
                if (enemy.IsPlayerInRange(player))
                {
                    enemy.AttackPlayer(player);
                }
                else
                {
                    enemy.MoveTowardsPlayer(player);
                }
            }
            else
            {
                Debug.LogError("Enemy in list is null!");
            }
        }
    }

    public void SpawnEnemies(int n, Vector3 spawnAreaCenter, float spawnRadius)
    {
        for (int i = 0; i < n; i++)
        {
            // ������ ��ġ�� �� ����
            Vector3 spawnPosition = spawnAreaCenter + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = 0; // 2D �����̶�� y�� 0���� ����

            GameObject enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            enemies.Add(enemy);
        }
        Debug.Log($"{n} enemies spawned.");
    }
}
