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
    // ���� - 
    // ������ state���� �ҷ���.
    // �÷��̾ �⺻������ �ҷ����� state. 
    // ������ �޾Ƽ� ���ŵ� cur_state.
    BuffState curState = null;
    [SerializeField]
    public BuffManager buffManager;

    public int damageSum = 0;
    //�ּ�ó���� �ڵ�� �ᵵ �ǰ� �Ƚᵵ�ǰ�..
    //public List<Ball> Balls; 
    public GameTurn currentTurn = GameTurn.DropBallState;

    void Start()
    {
        curState = new BuffState();
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
        buffManager.ShowBuffSelection(); // ���� ���� UI ǥ��

        // ������ ���õ� ������ ���
        yield return new WaitUntil(() => buffManager.IsBuffSelected());

        // ���õ� Buff ID ��� (�ʿ信 ���� �߰� �۾� ���� ����)
        int selectedBuffId = buffManager.GetSelectedBuffId();
        Debug.Log("Buff selected! ID: " + selectedBuffId);
    }

    private IEnumerator EndChkStage()
    {
        Debug.Log("Checking end conditions...");
        // Logic to check if the game should end or continue
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
        // ���ϵ��� 
        return true;
    }

    private bool buffChosen()
    {
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
        // ������ Ŭ���ϴ� �г��� �����ǰ�
        // �гο��� ������ ��ư��� ���� �Ŵ������� update�� �ɰ���.
        // �׷� ���� ��Ʈ�Ŵ������� �������� ���� �ʿ��ϳ�?
        // ��, ��ư�� Ŭ���ǰ� updateBuffState()�� ����Ǹ� return ���� 1 �ƴϸ� 0 
        //curState = buffManager.getBuffState();
        return true;
    }

    private bool chkStageEnded()
    {
        // ���ϵ��� 
        return true;
    }
}