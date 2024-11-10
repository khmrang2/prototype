using System.Collections.Generic;
using UnityEngine;

public class EnemyListManager : MonoBehaviour
{
    // 적들을 관리하는 리스트
    public GameObject enemyPrefab; // 생성할 적의 프리팹
    public List<Enemy> enemies = new List<Enemy>();

    // 적들의 행동을 실행하는 메서드
    public void ExecuteEnemyBehavior(Transform player)
    {
// enemies 리스트나 player가 null인지 확인
        if (enemies == null || player == null)
        {
            Debug.LogError("Enemies list or player reference is not assigned!");
            return;
        }

        foreach (var enemy in enemies)
        {
            if (enemy != null) // 적이 null이 아닌지 확인
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
            // 무작위 위치에 적 생성
            Vector3 spawnPosition = spawnAreaCenter + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = 0; // 2D 게임이라면 y를 0으로 고정

            GameObject enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            enemies.Add(enemy);
        }
        Debug.Log($"{n} enemies spawned.");
    }
}
