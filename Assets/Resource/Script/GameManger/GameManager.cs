using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 게임의 각 상황을 정의하고 제어하기 위해서
// 그냥 이름 지은 enum.
public enum GameTurn
{
    // 11일 (월) 비대면 회의 + 이슈 체크 회의
    // 
    // 13일 (수) 멘토님 컨펌
    // 태연
    DropBallState,          // 플레이어의 턴으로 공을 떨어뜨리는 상태 - 태연

    // 정훈님 
    PlayerAtkState,         // 떨어뜨린 공으로 적을 공격하는 상태

    // 시우님
    EnemyBehaviorState,     // 적의 턴으로 적이 행동(공격 or 움직임)하는 상태
    SpawnEnemyState,        // 적이 생성되는 상태

    // 현민
    EndChkState,            // 스테이지가 끝났는지(모든 적이 죽었는지) 체크하는 상태
    ChooseBuffState,        // 플레이어의 턴으로 버프를 선택하는 상태->>
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
    public Transform playerTransform;  // 플레이어의 Transform
    public EnemyListManager enemyListManager;  // EnemyListManager 참조
    // 현민 - 
    // 게임할 state들을 불러옴.
    // 플레이어가 기본적으로 불러오는 state. 
    // 버프를 받아서 갱신될 cur_state.
    [SerializeField]
    public BuffManager buffManager;
    
    BaseState buffState = null;
    BaseState defaultState = null;
    BaseState playerState = null;

    public int damageSum = 0;
    public GameTurn currentTurn = GameTurn.DropBallState;
    public PinManager pinManager; 

    void Start()
    {
        buffState = new BaseState();
        defaultState = new BaseState();
        playerState = new BaseState();
        // 게임을 시작할 프레임 워크의 시작.
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
        //플레이어 공격 투사체 생성
        //투사체는 스스로 나아가며 적과 접촉하거나 지정한 범위 밖으로 나가면 스스로 제거
        plAtkObj = Instantiate(prefPlayerAtkProjrctile);
        plAtkObj.transform.position = new Vector3(-2.4f, 4.85f, 0);
        Debug.Log("Player attacking...");
        yield return new WaitUntil(() => enemyAtkEnded());
    }

    private IEnumerator EnemyBehaviorTurn()
    {
        Debug.Log("Enemy Behavior...");
        // 적의 행동 (공격 또는 이동)을 처리하는 로직
        enemyListManager.HandleEnemyBehavior();  // 적이 플레이어를 향해 이동하거나 공격하도록 처리
        yield return new WaitUntil(() => enemyMoveEnded());
    }

    private IEnumerator SpawnEnemyTurn()
    {
        Debug.Log("Spawning enemies...");
        // 5초 간격으로 적 5명을 소환
        enemyListManager.SpawnEnemiesWithInterval();
        yield return new WaitUntil(() => spawnEnemyEnded());
    }

    private IEnumerator ChooseBuffTurn()
    {
        Debug.Log("Choosing a buff...");
        buffManager.ShowBuffSelection(); // 버프 선택 UI 표시

        // 버프가 선택될 때까지 대기
        yield return new WaitUntil(() => buffManager.IsBuffSelected());
        updateBuffState();
        Debug.Log("버프 업데이트됨.");
        buffState.printAllStates();
    }

    private IEnumerator EndChkStage()
    {
        Debug.Log("Checking end conditions...");
        yield return new WaitUntil(() => chkStageEnded());
    }

    public bool ballHasDropped()
    {
        return true;
    }
    private bool enemyAtkEnded()
    {
        if (plAtkObj == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool buffChosen()
    {
        return true;
    }

    private bool enemyMoveEnded()
    {
        return true; 
    }

    private bool spawnEnemyEnded()
    {
        return true;
    }

    private bool chooseBuffEnded()
    {
        // 이하동문 
        // 유저가 클릭하는 패널이 생성되고
        // 패널에서 선택한 버튼대로 버프 매니저에서 update가 될거임.
        // 그럼 이제 버트매니저에서 가져오는 것이 필요하네?
        // 즉, 버튼이 클릭되고 updateBuffState()가 실행되면 return 으로 1 아니면 0 
        //curState = buffManager.getBuffState();
        return true;
    }

    private bool chkStageEnded()
    {
        return true;  // 스테이지가 끝났는지를 체크하는 로직
    }

    public void updateBuffState()
    {
        this.buffState = buffManager.getBuffSumState();
    }
}