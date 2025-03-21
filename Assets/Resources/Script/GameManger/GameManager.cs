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
    public ProjectileOnHit plAtkObj;    //�÷��̾ ���� �� �߻��ϴ� ����ü
    public Transform playerTransform;  // �÷��̾��� Transform
    public EnemyListManager enemyListManager;  // EnemyListManager ����
    public GameObject clearPopup;       //���� Ŭ����� �����ϴ� �˾�
    // ���� - 
    // ������ state���� �ҷ���.
    // �÷��̾ �⺻������ �ҷ����� state. 
    // ������ �޾Ƽ� ���ŵ� cur_state.
    [SerializeField]
    public BuffManager buffManager;
    public PlayerManger playerManger;
    
    BaseState buffState = null;
    BaseState defaultState = null;
    BaseState playerState = null;

    public int pinHitCount = 0;
    public GameTurn currentTurn = GameTurn.DropBallState;
    public PinManager pinManager;
    public InteractionArea interactionArea;

    // �� ������ ������ ���۵Ǿ����� ���θ� üũ�ϴ� �÷���
    private bool stateStarted = false;
    private bool player_double_attack_chance = false;

    //���� �ý����� ������ ������Ű�� ���� ����
    public bool isPlaying = true;
    private bool isatkEnded = false;

    void Start()
    {
        clearPopup.SetActive(false);
        isPlaying = true;
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
                    int random_value = UnityEngine.Random.Range(0, 99);
                    if (random_value < playerManger.playerState.Player_DoubleUpChance) player_double_attack_chance = true;
                    stateStarted = true;
                    isatkEnded = false;
                    //���� �Լ� ȣ��
                    plAtkObj.StartAttack();
                    Debug.Log("Player attacking...");
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
                    //����ִ� ���� ���ٸ� ���� Ŭ���� ó��
                    if (enemyListManager.isAllEnemyDead())
                    {
                        stateStarted = true;    //���� ������ ���� ���� stateStarted�� ���� ������
                        isPlaying = false;

                        //���� Ŭ���� �˾� ����
                        clearPopup.SetActive(true);

                    }
                    else   //����ִ� ���� �ִٸ� ���� �̵� �۵�
                    {

                        Debug.Log("Enemies moving...");
                        // �� �̵��� ���� (enemyListManager.MoveEnemies()�� ���������� �̵��� ó���ϰ�,
                        // AllEnemiesMoved()�� �̵� �ϷḦ �Ǵ��Ѵٰ� ����)
                        enemyListManager.MoveEnemies();
                        stateStarted = true;
                    }

                }
                if (enemyListManager.AllEnemiesMoved() && isPlaying)
                {
                    enemyListManager.SpawnEnemyPerTurn();
                    stateStarted = false;
                    currentTurn = GameTurn.ChooseBuffState;
                }
                    
                
                break;

            case GameTurn.ChooseBuffState:
                if (playerManger.isAlive)
                {
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
                }
                break;

            case GameTurn.EndChkState:
                if (!stateStarted)
                {
                    Debug.Log("Checking end conditions...");
                    pinHitCount = 0;
                    interactionArea.init_ball();
                    stateStarted = true;
                }
                if (chkStageEnded())
                {
                    stateStarted = false;
                    pinManager.RespawnPins();
                    currentTurn = GameTurn.DropBallState;
                }
                break;
        }
    }
    public bool ballHasDropped()
    {
        //���� �� ������� �ÿ� ����
        if (interactionArea.get_ball_num() == 0 && GameObject.FindWithTag("Ball") == null)
        {
            pinHitCount = pinManager.hit_cnt_sum();
            Debug.Log("Total hit count: " + pinHitCount);
            pinManager.init_pins_hit_cnt();
            return true;
        }
        return false;
    }

    private bool enemyAtkEnded()
    {
        return isatkEnded;
    }

    private bool chkStageEnded()
    {
        // �������� ���� ���� üũ ���� (�ʿ信 ���� ����)
        return true;
    }

    public void updateBuffState()
    {
        return;
    }

    // ����ü�� ���ŵ� �� ȣ��Ǿ� ���¸� ������Ʈ
    public void NotifyProjectileDestroyed()
    {
        if (player_double_attack_chance)
        {
            player_double_attack_chance = false;  // 더블 어택 기회 소진
            plAtkObj.StartAttack();  // 다시 발사
        }
        else
        {
            isatkEnded = true; // 더 이상 공격할 필요 없음 → 공격 턴 종료
        }
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
