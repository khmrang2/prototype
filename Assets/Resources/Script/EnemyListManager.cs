using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyListManager : MonoBehaviour
{
    // 적 프리팹을 담을 변수 (Unity 에디터에서 할당)
    //public GameObject enemyPrefab;
    public Transform playerTransform;
    public Transform enemyInitialTransform; // 몬스터가 처음 생성되는 기준 위치
    public Transform enemySpawnTransform;   //몬스터가 스폰 처리 시 이동 될 위치
    private List<Enemy> enemies = new List<Enemy>();

    [SerializeField] private int turn;  //턴 값

    public int maxEnemies = 0; // 등장할 적의 총 수

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
        if (!enemiesSpawned)
        {
            for (int i = 0; i < maxEnemies; i++)
            {
                GameObject enemyObject = Instantiate(enemyDataList.EnemyList[i], enemyInitialTransform.position, Quaternion.identity);
                Enemy enemy = enemyObject.GetComponent<Enemy>();
                enemies.Add(enemy);

                EnemyStatus enemyStatus = enemyObject.GetComponent<EnemyStatus>();
                //생성된 각 적들에게 스크립터블 오브젝트를 참조하여 각자의 스탯 부여
                enemyStatus.SetEnemyStat(enemyDataList.EnemyHPList[i], enemyDataList.EnemyATKList[i], enemyDataList.EnemyDFFList[i]);
            }
            enemiesSpawned = true; // 한 번만 실행되도록 설정

            //게임 시작하자마자 소환되어야 하는 적들의 소환 처리
            SpawnEnemyPerTurn();

        }
    }

    // === 2?? 모든 적이 플레이어를 향해 이동 ===
    public async Task MoveEnemies()
    {

        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
               await enemy.Move(); // 적 이동 및 공격 실행, 이 과정이 끝날 때까지 대기 후 진행
            }
        }
        turn += 1;  //호출이 완료되었으면 턴이 끝난 것이므로 턴 +1

        //이동이 끝나고 턴이 지났으니 스폰처리
        //SpawnEnemyPerTurn();
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
            int willspawncnt = enemyDataList.EnemySpawnCountPerTurn[turn];

            for (int i = spawnedCount; i < spawnedCount + willspawncnt; i++)
            {
                enemies[i].transform.position = enemySpawnTransform.position;
                enemies[i].isSpawned = true;
            }

            spawnedCount += willspawncnt;
        }
    }

}
