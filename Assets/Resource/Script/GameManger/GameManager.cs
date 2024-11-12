using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ������ �� ��Ȳ�� �����ϰ� �����ϱ� ���ؼ�
// �׳� �̸� ���� enum.
public enum GameTurn
{
    // 11�� (��) ���� ȸ�� + �̽� üũ ȸ��
    // 
    // 13�� (��) ����� ����
                            // �¿�
    DropBallState,          // �÷��̾��� ������ ���� ����߸��� ���� - �¿�

                            // ���ƴ� 
    PlayerAtkState,         // ����߸� ������ ���� �����ϴ� ����
                            
                            // �ÿ��
    EnemyBehaviorState,     // ���� ������ ���� �ൿ(���� or ������)�ϴ� ����
    SpawnEnemyState,        // ���� �����Ǵ� ����

                            // ����
    EndChkState,            // ���������� ��������(��� ���� �׾�����) üũ�ϴ� ����
    ChooseBuffState,        // �÷��̾��� ������ ������ �����ϴ� ����->>
}
public class GameManager : MonoBehaviour
{
    public GameObject prefPlayerAtkProjrctile;
    private GameObject plAtkObj;
    public Transform playerTransform;  // �÷��̾��� Transform
    public EnemyListManager enemyListManager;  // EnemyListManager ����
    public int damageSum = 0;
    public GameTurn currentTurn = GameTurn.DropBallState;

    void Start()
    {
        // ���� ���� ����
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            switch (currentTurn)
            {
                case GameTurn.DropBallState:
                    yield return StartCoroutine(DropBallTurn());
                    currentTurn = GameTurn.PlayerAtkState;
                    break;
                case GameTurn.PlayerAtkState:
                    yield return StartCoroutine(PlayerAtkTurn());
                    currentTurn = GameTurn.EnemyBehaviorState;
                    break;
                case GameTurn.EnemyBehaviorState:
                    yield return StartCoroutine(EnemyBehaviorTurn());
                    currentTurn = GameTurn.SpawnEnemyState;
                    break;
                case GameTurn.SpawnEnemyState:
                    yield return StartCoroutine(SpawnEnemyTurn());
                    currentTurn = GameTurn.ChooseBuffState;
                    break;
                case GameTurn.ChooseBuffState:
                    yield return StartCoroutine(ChooseBuffTurn());
                    currentTurn = GameTurn.EndChkState;
                    break;
                case GameTurn.EndChkState:
                    yield return StartCoroutine(EndChkStage());
                    currentTurn = GameTurn.DropBallState;
                    break;
            }
        }
    }

    private IEnumerator DropBallTurn()
    {
        Debug.Log("Dropping ball...");
        yield return new WaitUntil(() => ballHasDropped());
    }

    private IEnumerator PlayerAtkTurn()
    {
        //�÷��̾� ���� ����ü ����
        //����ü�� ������ ���ư��� ���� �����ϰų� ������ ���� ������ ������ ������ ����
        plAtkObj = Instantiate(prefPlayerAtkProjrctile);
        plAtkObj.transform.position = new Vector3(-2.4f, 4.85f, 0);
        Debug.Log("Player attacking...");
        yield return new WaitUntil(() => enemyAtkEnded());
    }

    private IEnumerator EnemyBehaviorTurn()
    {
        Debug.Log("Enemy Behavior...");
        // ���� �ൿ (���� �Ǵ� �̵�)�� ó���ϴ� ����
        enemyListManager.HandleEnemyBehavior();  // ���� �÷��̾ ���� �̵��ϰų� �����ϵ��� ó��
        yield return new WaitUntil(() => enemyMoveEnded());
    }

    private IEnumerator SpawnEnemyTurn()
    {
        Debug.Log("Spawning enemies...");
        // 5�� �������� �� 5���� ��ȯ
        enemyListManager.SpawnEnemiesWithInterval();
        yield return new WaitUntil(() => spawnEnemyEnded());
    }

    private IEnumerator ChooseBuffTurn()
    {
        Debug.Log("Choosing a buff...");
        yield return new WaitUntil(() => chooseBuffEnded());
    }

    private IEnumerator EndChkStage()
    {
        Debug.Log("Checking end conditions...");
        yield return new WaitUntil(() => chkStageEnded());
    }

    private bool ballHasDropped()
    {
        // ���� ���⼭ ���� ��� ������������ üũ�Ͽ� return ���ָ� �˴ϴ�.
        // balls[]��� ����Ʈ�� �����ؼ� ��� ���� ������� �� BallHasDropped�� invoke���ְų�
        // �ƴϸ� ������ ���� ������ٸ� �� �ڵ带 invoke �ϴ� �������� �������ֽø� �˴ϴ�.
        return true;
    }
    private bool enemyAtkEnded()
    {
        //����ü ���� �浹�ϰų� ȭ�� ������ ������ ����
        //����ü ��� ������ null�� �Ǿ��ٸ� �÷��̾� ���� ����� �Ǵ��ϰ� true ��ȯ
        // �ƴϸ� false ��ȯ

        if (plAtkObj == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool enemyMoveEnded()
    {
        return true;  // ���� �̵��� ���������� üũ�ϴ� ����
    }

    private bool spawnEnemyEnded()
    {
        return true;  // ���� ��� ��ȯ�ƴ����� üũ�ϴ� ����
    }

    private bool chooseBuffEnded()
    {
        return true;  // ������ ���õƴ����� üũ�ϴ� ����
    }

    private bool chkStageEnded()
    {
        return true;  // ���������� ���������� üũ�ϴ� ����
    }
}