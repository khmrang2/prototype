using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyListManager : MonoBehaviour
{
    public PlayerState playerState;

    // 적 프리팹을 담을 변수 (Unity 에디터에서 할당)
    //public GameObject enemyPrefab;
    public Transform playerTransform; //몬스터가 스폰 처리 시 이동 될 위치
    public Transform enemySpawnTransform;
    public Transform enemyStartTransform;
    public List<Enemy> enemies = new List<Enemy>();
    public GameObject EnemyHpbarCanvas;

    [SerializeField] private int turn;  //턴 값

    [SerializeField] private int maxEnemies = 0; // 등장할 적의 총 수

    private int spawnedCount;   //등장한 적의 수

    private bool enemiesSpawned = false; // 적이 한 번만 스폰되도록 체크

    private bool allMoved = false;

    [SerializeField] private EnemyDataList enemyDataList;   //해당 맵에서 등장할 적들의 스탯이 담긴 스크립터블 오브젝스

    public GameManager gameManager;
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
            enemy.start = enemySpawnTransform;
            EnemyStatus enemyStatus = enemyObject.GetComponent<EnemyStatus>();
            //생성된 각 적들에게 스크립터블 오브젝트를 참조하여 각자의 스탯 부여
            enemyStatus.SetEnemyStat((int)(1.0-playerState.Enemy_Health) * enemyDataList.EnemyList[i].hp, (int)(1.0 - playerState.Enemy_Attack) * enemyDataList.EnemyList[i].attack, enemyDataList.EnemyList[i].defense);
            //enemyObject.SetActive(false);
            //적 체력바 소환
            enemy.canvas = EnemyHpbarCanvas;
            enemy.SetEnemyHealthBar();
            enemy.playerState = playerState;

        }
        enemiesSpawned = true; // 한 번만 실행되도록 설정

        //게임 시작하자마자 소환되어야 하는 적들의 소환 처리
        SpawnEnemyPerTurn();
    }

    public async Task MoveEnemies()
    {
        allMoved = false;
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
                Debug.Log($"{enemy.name} : 의 행동이 끝났따./!");
            }
        }
        turn += 1; // 한 번 호출될 때마다 1턴 증가
        //이동이 끝나고 턴이 지났으니 스폰처리
        //SpawnEnemyPerTurn()
        // 이동이 끝나면, 적 이동 처리 완료
        allMoved = true;
        Debug.Log("All enemies moved.");
    }



    // === 3?? 모든 적의 이동이 끝났는지 확인 ===
    public bool AllEnemiesMoved()
    {
        if (allMoved) return true;
        else return false;
        /*
        foreach (var enemy in enemies)
        {
            Debug.Log($"{enemy.name} 움직임 체크.");
            // enemy가 존재하고, 움직이는 중이면.
            // enemy가 존재하지않으면? enemy == null이면?
            if (enemy == null) {
                Debug.Log("이 enemy는 존재하지 않음. 넘어감.");
                continue;
            }
            if (enemy != null && !enemy.HasMoved())
            {
                Debug.Log($"{enemy.name}이 움직이는 중임.");
                return false; // 아직 이동 안 한 적이 있음
            }
        }
        Debug.Log($"모든 적 이동 완료.");
        return true; // 모든 적 이동 완료*/
        
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
        // 디버그용으로 주석처리한거. 나중에 커밋할때 꼭 
        if (spawnedCount < maxEnemies)
        {
            Debug.LogWarning($"{spawnedCount} < {maxEnemies}");
            int willSpawnCnt = enemyDataList.EnemySpawnCountPerTurn[turn];
            Debug.LogWarning($"{spawnedCount} < {willSpawnCnt}");

            for (int i = spawnedCount; i < spawnedCount + willSpawnCnt; i++)
            {
                enemies[i].transform.position = enemySpawnTransform.position + enemies[i].spawn_offset;
                enemies[i].isSpawned = true;
                gameManager.Pin_Damage_Event += enemies[i].Get_Pin_Damage;
            }

            spawnedCount += willSpawnCnt;
            Debug.LogWarning($"스폰이 잘 이루어짐 : {spawnedCount}");
        }
    }
    public List<Enemy> GetEnemies()
    {
        return enemies;
    }


    // === 6?? 게임 클리어 여부 확인을 위한 적 생존 확인 ===
    public bool isAllEnemyDead()
    {
        foreach(var enemy in enemies)
        {
            if(enemy.isAlive == true) return false;     //하나라도 살아있다면 false 반환
        }
        Debug.Log("다 죽었다");
        return true;    //다 죽었다면 true 반환
    }

}
