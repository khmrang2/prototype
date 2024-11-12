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


// ���� ��Ȳ�� �����ϱ� ���� ���� ����ü.
struct buffState
{
    int numberOfBalls;
    //int spawnOffset;
    int damageOfBall;
};

public class GameManager : MonoBehaviour
{
    public int damageSum = 0;
    //�ּ�ó���� �ڵ�� �ᵵ �ǰ� �Ƚᵵ�ǰ�..
    //public List<Ball> Balls; 
    public GameTurn currentTurn = GameTurn.DropBallState;
    public PinManager pinManager; 

    void Start()
    {
        // ������ ������ ������ ��ũ�� ����.
        StartCoroutine(GameLoop());
    }

    // 
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
        // Drop ball logic goes here
        yield return new WaitUntil(() => ballHasDropped());
    }

    private IEnumerator PlayerAtkTurn()
    {
        Debug.Log("player attacking...");
        // Enemy attack logic goes here
        yield return new WaitUntil(() => enemyAtkEnded());
    }

    private IEnumerator EnemyBehaviorTurn()
    {
        Debug.Log("Enemy Behavior...");
        // Logic for enemy movement
        yield return new WaitUntil(() => enemyMoveEnded());
    }

    private IEnumerator SpawnEnemyTurn()
    {
        Debug.Log("Spawning enemy...");
        // Logic for spawning enemy units
        yield return new WaitUntil(() => spawnEnemyEnded());
    }

    private IEnumerator ChooseBuffTurn()
    {
        Debug.Log("Choosing a buff...");
        // Logic for choosing a buff (e.g., wait for player interaction)
        yield return new WaitUntil(() => chooseBuffEnded());
    }

    private IEnumerator EndChkStage()
    {
        Debug.Log("Checking end conditions...");
        // Logic to check if the game should end or continue
        yield return new WaitUntil(() => chkStageEnded());
    }

    public bool ballHasDropped()
    {
        return true;
    }
    private bool enemyAtkEnded()
    {
        // ���ϵ��� 
        return true;
    }

    private bool buffChosen()
    {
        // ������ ���� ������ ���õǰ� 
        // ��Ƽ��Ʈ�� �����ϴ� �ڵ忡�� �� �ڵ带 �κ�ũ ���ָ�
        // ���� ���·� �Ѿ �� �ֽ��ϴ�. 
        return true;
    }

    private bool enemyMoveEnded()
    {
        // ���ϵ��� 
        return true;
    }
    private bool spawnEnemyEnded()
    {
        // ���ϵ��� 
        return true;
    }
    private bool chooseBuffEnded()
    {
        // ���ϵ��� 
        return true;
    }

    private bool chkStageEnded()
    {
        // ���ϵ��� 
        return true;
    }
}