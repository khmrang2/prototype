using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyListManager : MonoBehaviour
{
    // 적 프리팹을 담을 변수 (Unity 에디터에서 할당)
    public GameObject enemyPrefab;
    public Transform playerTransform;
    public Transform enemyTransform; // 몬스터가 생성되는 기준 위치
    private List<Enemy> enemies = new List<Enemy>();
    public int maxEnemies = 5; // 최대 5마리
    public float spawnInterval = 5f; // 5초 간격으로 몬스터 생성
    void Start()
    {
        // 몬스터를 생성하는 코루틴 시작
        StartCoroutine(SpawnEnemiesWithInterval());
    }
    public IEnumerator SpawnEnemiesWithInterval()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            // 적을 생성하고 리스트에 추가
            GameObject enemyObject = Instantiate(enemyPrefab, enemyTransform.position, Quaternion.identity);
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            enemies.Add(enemy);

            // 5초 대기 후 다음 적 생성
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    // 모든 적의 행동을 처리하는 메서드
    public void HandleEnemyBehavior()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.Act(playerTransform); // 각 적이 플레이어를 향해 이동하도록 함
            }
        }
    }
}
