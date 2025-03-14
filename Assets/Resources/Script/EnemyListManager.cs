using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyListManager : MonoBehaviour
{
    // �� �������� ���� ���� (Unity �����Ϳ��� �Ҵ�)
    //public GameObject enemyPrefab;
    public Transform playerTransform; //���Ͱ� ���� ó�� �� �̵� �� ��ġ
    public Transform enemySpawnTransform;
    public Transform enemyStartTransform;
    private List<Enemy> enemies = new List<Enemy>();

    [SerializeField] private int turn;  //�� ��

    [SerializeField] private int maxEnemies = 0; // ������ ���� �� ��

    private int spawnedCount;   //������ ���� ��

    private bool enemiesSpawned = false; // ���� �� ���� �����ǵ��� üũ

    [SerializeField] private EnemyDataList enemyDataList;   //�ش� �ʿ��� ������ ������ ������ ��� ��ũ���ͺ� ��������

    

    //�ʱ�ȭ �Լ�
    private void Awake()
    {
        turn = 0;
        maxEnemies = enemyDataList.EnemyList.Count;
        spawnedCount = 0;
    }

    // === 1?? ���� ���� �� ó�� �� ���� �� ���� ===
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
            //������ �� ���鿡�� ��ũ���ͺ� ������Ʈ�� �����Ͽ� ������ ���� �ο�
            enemyStatus.SetEnemyStat(enemyDataList.EnemyList[i].hp, enemyDataList.EnemyList[i].attack, enemyDataList.EnemyList[i].defense);
            //enemyObject.SetActive(false);
        }
        enemiesSpawned = true; // �� ���� ����ǵ��� ����

        //���� �������ڸ��� ��ȯ�Ǿ�� �ϴ� ������ ��ȯ ó��
        SpawnEnemyPerTurn();
    }

    public async Task MoveEnemies()
    {
        // �� ������ 5���� ���� ���� �̵��ϵ��� ó��
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                // �÷��̾ ���� �̵� ����
                enemy.MoveOneStep();

                // 5���� �̵� �ܰ� �Ϸ�� ������ ��ٸ�
                while (enemy.isMoving)
                {
                    await Task.Yield();  // �񵿱������� �̵��� ���� ������ ���
                }
            }
        }
        turn += 1; // �� �� ȣ��� ������ 1�� ����
        //�̵��� ������ ���� �������� ����ó��
        //SpawnEnemyPerTurn()
        // �̵��� ������, �� �̵� ó�� �Ϸ�
        Debug.Log("All enemies moved.");
    }



    // === 3?? ��� ���� �̵��� �������� Ȯ�� ===
    public bool AllEnemiesMoved()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null && !enemy.HasMoved())
            {
                //Debug.Log("still moving");
                return false; // ���� �̵� �� �� ���� ����
            }
        }
        
        return true; // ��� �� �̵� �Ϸ�
        
    }

    // === 4?? ���� ���� ���� �� �̵� ���� �ʱ�ȭ ===
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

    // === 5?? �Ͽ� ���� �� ���� ó�� ===
    public void SpawnEnemyPerTurn() 
    {
        Debug.Log("spawning enemy-------------------------------------------------------------------");

        //�� ���� ������ ���� ��ȯ�� ���� ���� ���� ���� ����
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
