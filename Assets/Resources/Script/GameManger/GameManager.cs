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

    void Start()
    {
        buffState = new BaseState();
        defaultState = new BaseState();
        playerState = new BaseState();
        // ������ ������ ������ ��ũ�� ����.
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        enemyListManager.SpawnInitialEnemies();
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
        Debug.Log("Enemies moving...");
        enemyListManager.MoveEnemies(); // ���� �̵��ϵ��� ȣ��

        yield return new WaitUntil(() => enemyMoveEnded());
        currentTurn = GameTurn.ChooseBuffState;
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

    public bool ballHasDropped()
    {
        if (interactionArea.get_ball_num() == 0 && GameObject.FindWithTag("Ball") == null)
        {
            // pinManager?�서 ?�산??hit count�?damageSum???�??
            damageSum = pinManager.hit_cnt_sum();
            Debug.Log("Total hit count: " + damageSum);

            pinManager.init_pins_hit_cnt();

            // true 반환
            return true;
        }
        return false;
    }
    private bool enemyAtkEnded()
    {
        return plAtkObj == null;
    }

    private bool buffChosen()
    {
        return true;
    }

    private bool enemyMoveEnded()
    {
        return enemyListManager.AllEnemiesMoved();
    }

    private bool spawnEnemyEnded()
    {
        return true;
    }

    private bool chooseBuffEnded()
    {
        // ���ϵ��� 
        // ������ Ŭ���ϴ� �г��� �����ǰ�
        // �гο��� ������ ��ư��� ���� �Ŵ������� update�� �ɰ���.
        // �׷� ���� ��Ʈ�Ŵ������� �������� ���� �ʿ��ϳ�?
        // ��, ��ư�� Ŭ���ǰ� updateBuffState()�� ����Ǹ� return ���� 1 �ƴϸ� 0 
        //curState = buffManager.getBuffState();
        return true;
    }

    private bool chkStageEnded()
    {
        return true;  // ���������� ���������� üũ�ϴ� ����
    }

    public void updateBuffState()
    {
        this.buffState = buffManager.getBuffSumState();
    }

    // ����ü�� ���ŵ� �� GameManager�� �˸�
    public void NotifyProjectileDestroyed()
    {
        plAtkObj = null;
    }
}