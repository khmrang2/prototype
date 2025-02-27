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
    private bool enemiesSpawned = false; // ���� �� ���� �����ǵ��� üũ

    [SerializeField] private EnemyDataList enemyDataList;   //�ش� �ʿ��� ������ ������ ������ ��� ��ũ���ͺ� ��������

    // === 1?? ���� ���� �� ó�� �� ���� �� ���� ===
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
                //������ �� ���鿡�� ��ũ���ͺ� ������Ʈ�� �����Ͽ� ������ ���� �ο�
                enemyStatus.SetEnemyStat(enemyDataList.EnemyHPList[i], enemyDataList.EnemyATKList[i], enemyDataList.EnemyDFFList[i]);
            }
            enemiesSpawned = true; // �� ���� ����ǵ��� ����
        }
    }

    // === 2?? ��� ���� �÷��̾ ���� �̵� ===
    public void MoveEnemies()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.Move(); // �� �̵� ����
            }
        }
    }

    // === 3?? ��� ���� �̵��� �������� Ȯ�� ===
    public bool AllEnemiesMoved()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null && !enemy.HasMoved())
            {
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
}
