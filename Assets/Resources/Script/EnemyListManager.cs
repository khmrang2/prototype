using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyListManager : MonoBehaviour
{
    // �� �������� ���� ���� (Unity �����Ϳ��� �Ҵ�)
    //public GameObject enemyPrefab;
    public Transform playerTransform;
    public Transform enemyInitialTransform; // ���Ͱ� ó�� �����Ǵ� ���� ��ġ
    public Transform enemySpawnTransform;   //���Ͱ� ���� ó�� �� �̵� �� ��ġ
    private List<Enemy> enemies = new List<Enemy>();

    [SerializeField] private int turn;  //�� ��

    public int maxEnemies = 0; // ������ ���� �� ��

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
        if (!enemiesSpawned)
        {
            for (int i = 0; i < maxEnemies; i++)
            {
                GameObject enemyObject = Instantiate(enemyDataList.EnemyList[i], enemyInitialTransform.position, Quaternion.identity);
                Enemy enemy = enemyObject.GetComponent<Enemy>();
                enemies.Add(enemy);

                EnemyStatus enemyStatus = enemyObject.GetComponent<EnemyStatus>();
                //������ �� ���鿡�� ��ũ���ͺ� ������Ʈ�� �����Ͽ� ������ ���� �ο�
                enemyStatus.SetEnemyStat(enemyDataList.EnemyHPList[i], enemyDataList.EnemyATKList[i], enemyDataList.EnemyDFFList[i]);
            }
            enemiesSpawned = true; // �� ���� ����ǵ��� ����

            //���� �������ڸ��� ��ȯ�Ǿ�� �ϴ� ������ ��ȯ ó��
            SpawnEnemyPerTurn();

        }
    }

    // === 2?? ��� ���� �÷��̾ ���� �̵� ===
    public async Task MoveEnemies()
    {

        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
               await enemy.Move(); // �� �̵� �� ���� ����, �� ������ ���� ������ ��� �� ����
            }
        }
        turn += 1;  //ȣ���� �Ϸ�Ǿ����� ���� ���� ���̹Ƿ� �� +1

        //�̵��� ������ ���� �������� ����ó��
        //SpawnEnemyPerTurn();
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
