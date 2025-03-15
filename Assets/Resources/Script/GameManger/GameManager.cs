using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ������ �� ��Ȳ�� �����ϰ� �����ϱ� ���ؼ�
// �׳� �̸� ���� enum.
public enum GameTurn
{
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
struct buffState
{
    int numberOfBalls;
    //int spawnOffset;
    int damageOfBall;
};
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject prefPlayerAtkProjrctile;
    private GameObject plAtkObj;
    public Transform playerTransform;  // �÷��̾��� Transform
    public EnemyListManager enemyListManager;  // EnemyListManager ����
    // ���� - 
    // ������ state���� �ҷ���.
    // �÷��̾ �⺻������ �ҷ����� state. 
    // ������ �޾Ƽ� ���ŵ� cur_state.
    [SerializeField]
    public BuffManager buffManager;
    
    BaseState buffState = null;
    BaseState defaultState = null;
    BaseState playerState = null;

    public int damageSum = 0;
    public GameTurn currentTurn = GameTurn.DropBallState;
    public PinManager pinManager;
    public InteractionArea interactionArea;

    // �� ������ ������ ���۵Ǿ����� ���θ� üũ�ϴ� �÷���
    private bool stateStarted = false;

    void Start()
    {
        buffState = new BaseState();
        defaultState = new BaseState();
        playerState = new BaseState();
        enemyListManager.SpawnInitialEnemies();
    }
    /// <summary>
    /// stateStarted�� ���ɶ��� �����Ͽ�
    /// ���� ������.
    /// </summary>

    void Update()
    {
        switch (currentTurn)
        {
            case GameTurn.DropBallState:
                if (!stateStarted)
                {
                    Debug.Log("Dropping ball...");
                    stateStarted = true;
                }
                if (ballHasDropped())
                {
                    stateStarted = false;
                    currentTurn = GameTurn.PlayerAtkState;
                }
                break;

            case GameTurn.PlayerAtkState:
                if (!stateStarted)
                {
                    plAtkObj = Instantiate(prefPlayerAtkProjrctile);
                    plAtkObj.transform.position = new Vector3(-2.4f, 4.85f, 0);
                    Debug.Log("Player attacking...");
                    stateStarted = true;
                }
                if (enemyAtkEnded())
                {
                    stateStarted = false;
                    currentTurn = GameTurn.EnemyBehaviorState;
                }
                break;

            case GameTurn.EnemyBehaviorState:
                if (!stateStarted)
                {
                    Debug.Log("Enemies moving...");
                    // �� �̵��� ���� (enemyListManager.MoveEnemies()�� ���������� �̵��� ó���ϰ�,
                    // AllEnemiesMoved()�� �̵� �ϷḦ �Ǵ��Ѵٰ� ����)
                    enemyListManager.MoveEnemies();
                    stateStarted = true;
                }
                if (enemyListManager.AllEnemiesMoved())
                {
                    enemyListManager.SpawnEnemyPerTurn();
                    stateStarted = false;
                    currentTurn = GameTurn.ChooseBuffState;
                }
                break;

            case GameTurn.ChooseBuffState:
                if (!stateStarted)
                {
                    Debug.Log("Choosing a buff...");
                    buffManager.ShowBuffSelection();
                    stateStarted = true;
                }
                if (buffManager.IsBuffSelected())
                {
                    updateBuffState();
                    Debug.Log("Buff updated.");
                    buffState.printAllStates();
                    stateStarted = false;
                    currentTurn = GameTurn.EndChkState;
                }
                break;

            case GameTurn.EndChkState:
                if (!stateStarted)
                {
                    Debug.Log("Checking end conditions...");
                    damageSum = 0;
                    interactionArea.init_ball();
                    stateStarted = true;
                }
                if (chkStageEnded())
                {
                    stateStarted = false;
                    currentTurn = GameTurn.DropBallState;
                }
                break;
        }
    }
    public bool ballHasDropped()
    {
        if (interactionArea.get_ball_num() == 0 && GameObject.FindWithTag("Ball") == null)
        {
            damageSum = pinManager.hit_cnt_sum();
            Debug.Log("Total hit count: " + damageSum);
            pinManager.init_pins_hit_cnt();
            return true;
        }
        return false;
    }

    private bool enemyAtkEnded()
    {
        return plAtkObj == null;
    }

    private bool chkStageEnded()
    {
        // �������� ���� ���� üũ ���� (�ʿ信 ���� ����)
        return true;
    }

    public void updateBuffState()
    {
        this.buffState = buffManager.getBuffSumState();
    }

    // ����ü�� ���ŵ� �� ȣ��Ǿ� ���¸� ������Ʈ
    public void NotifyProjectileDestroyed()
    {
        plAtkObj = null;
    }
}

/**
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
        Debug.Log("Enemies moving...");

        // ���� 5ĭ�� ������ �̵���Ű��
        yield return enemyListManager.MoveEnemies();

        yield return new WaitUntil(() => enemyListManager.AllEnemiesMoved());

        // �̵��� ������ ���� ó��
        enemyListManager.SpawnEnemyPerTurn();
    }

    private IEnumerator MoveEnemiesCoroutine()
    {
        var moveEnemiesTask = enemyListManager.MoveEnemies(); // MoveEnemies() ����
        yield return new WaitUntil(() => moveEnemiesTask.IsCompleted); // �Ϸ�� ������ ���
    }

    private IEnumerator ChooseBuffTurn()
    {
        Debug.Log("Choosing a buff...");
        buffManager.ShowBuffSelection(); // ���� ���� UI ǥ��

        // ������ ���õ� ������ ���
        yield return new WaitUntil(() => buffManager.IsBuffSelected());
        updateBuffState();
        Debug.Log("���� ������Ʈ��.");
        buffState.printAllStates();
    }

    private IEnumerator EndChkStage()
    {
        Debug.Log("Checking end conditions...");
        damageSum = 0;
        interactionArea.init_ball();
        yield return new WaitUntil(() => chkStageEnded());
    }
 
 */
