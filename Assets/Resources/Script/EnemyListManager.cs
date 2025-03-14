using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyListManager : MonoBehaviour
{
    // 적 프리팹을 담을 변수 (Unity 에디터에서 할당)
    //public GameObject enemyPrefab;
    public Transform playerTransform; //몬스터가 스폰 처리 시 이동 될 위치
    public Transform enemySpawnTransform;
    public Transform enemyStartTransform;
    private List<Enemy> enemies = new List<Enemy>();

    [SerializeField] private int turn;  //턴 값

    [SerializeField] private int maxEnemies = 0; // 등장할 적의 총 수

    private int spawnedCount;   //등장한 적의 수

    private bool enemiesSpawned = false; // 적이 한 번만 스폰되도록 체크

    [SerializeField] private EnemyDataList enemyDataList;   //해당 맵에서 등장할 적들의 스탯이 담긴 스크립터블 오브젝스

    

    //초기화 함수
    private void Awake()
    {
        turn = 0;
        maxEnemies = enemyDataList.EnemyList.Count;
        spawnedCount = 0;
    }

    // === 1?? 게임 시작 시 처음 한 번만 적 스폰 ===
    public void SpawnInitialEnemies()
    {
        if (enemiesSpawned)
            return;

        for (int i = 0; i < maxEnemies; i++)
        {
            GameObject enemyObject = Instantiate(enemyDataList.EnemyList[i].enemyType, enemyStartTransform.position, Quaternion.identity);
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            enemies.Add(enemy);
            EnemyStatus enemyStatus = enemyObject.GetComponent<EnemyStatus>();
            //생성된 각 적들에게 스크립터블 오브젝트를 참조하여 각자의 스탯 부여
            enemyStatus.SetEnemyStat(enemyDataList.EnemyList[i].hp, enemyDataList.EnemyList[i].attack, enemyDataList.EnemyList[i].defense);
            //enemyObject.SetActive(false);
        }
        enemiesSpawned = true; // 한 번만 실행되도록 설정

        //게임 시작하자마자 소환되어야 하는 적들의 소환 처리
        SpawnEnemyPerTurn();
    }

    public async Task MoveEnemies()
    {
        // 각 적마다 5번의 턴을 걸쳐 이동하도록 처리
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                // 플레이어를 향해 이동 시작
                enemy.MoveOneStep();

                // 5번의 이동 단계 완료될 때까지 기다림
                while (enemy.isMoving)
                {
                    await Task.Yield();  // 비동기적으로 이동이 끝날 때까지 대기
                }
            }
        }
        turn += 1; // 한 번 호출될 때마다 1턴 증가
        //이동이 끝나고 턴이 지났으니 스폰처리
        //SpawnEnemyPerTurn()
        // 이동이 끝나면, 적 이동 처리 완료
        Debug.Log("All enemies moved.");
    }



    // === 3?? 모든 적의 이동이 끝났는지 확인 ===
    public bool AllEnemiesMoved()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null && !enemy.HasMoved())
            {
                //Debug.Log("still moving");
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

    // === 5?? 턴에 따른 적 스폰 처리 ===
    public void SpawnEnemyPerTurn() 
    {
        Debug.Log("spawning enemy-------------------------------------------------------------------");

        //총 적의 수보다 현재 소환된 적의 수가 적을 때만 실행
        if (spawnedCount < maxEnemies)
        {
            int willSpawnCnt = enemyDataList.EnemySpawnCountPerTurn[turn];

            for (int i = spawnedCount; i < spawnedCount + willSpawnCnt; i++)
            {
                enemies[i].transform.position = enemySpawnTransform.position;
                enemies[i].isSpawned = true;
            }

            spawnedCount += willSpawnCnt;
        }
    }
    public List<Enemy> GetEnemies()
    {
        return enemies;
    }
}
