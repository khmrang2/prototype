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
    private bool enemiesSpawned = false; // 적이 한 번만 스폰되도록 체크

    [SerializeField] private EnemyDataList enemyDataList;   //해당 맵에서 등장할 적들의 스탯이 담긴 스크립터블 오브젝스

    // === 1?? 게임 시작 시 처음 한 번만 적 스폰 ===
    public void SpawnInitialEnemies()
    {
        if (!enemiesSpawned)
        {
            for (int i = 0; i < maxEnemies; i++)
            {
                GameObject enemyObject = Instantiate(enemyPrefab, enemyTransform.position, Quaternion.identity);
                Enemy enemy = enemyObject.GetComponent<Enemy>();
                enemies.Add(enemy);

                EnemyStatus enemyStatus = enemyObject.GetComponent<EnemyStatus>();
                //생성된 각 적들에게 스크립터블 오브젝트를 참조하여 각자의 스탯 부여
                enemyStatus.SetEnemyStat(enemyDataList.EnemyHPList[i], enemyDataList.EnemyATKList[i], enemyDataList.EnemyDFFList[i]);
            }
            enemiesSpawned = true; // 한 번만 실행되도록 설정
        }
    }

    // === 2?? 모든 적이 플레이어를 향해 이동 ===
    public void MoveEnemies()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.Move(); // 적 이동 실행
            }
        }
    }

    // === 3?? 모든 적의 이동이 끝났는지 확인 ===
    public bool AllEnemiesMoved()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null && !enemy.HasMoved())
            {
                return false; // 아직 이동 안 한 적이 있음
            }
        }
        return true; // 모든 적 이동 완료
    }

    // === 4?? 다음 턴을 위해 적 이동 여부 초기화 ===
    public void ResetEnemyMoves()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.ResetMove();
            }
        }
    }
}
