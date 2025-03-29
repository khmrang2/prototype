using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameTurn
{
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
    public static GameManager Instance { get; private set; }
    
    public Transform playerTransform;  // 플레이어의 Transform
    public EnemyListManager enemyListManager;  // EnemyListManager 참조
    public GameObject clearPopup;       //게임 클리어시 등장하는 팝업
    // 현민 - 
    // 게임할 state들을 불러옴.
    // 플레이어가 기본적으로 불러오는 state. 
    // 버프를 받아서 갱신될 cur_state.
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

    // 각 상태의 동작이 시작되었는지 여부를 체크하는 플래그
    private bool stateStarted = false;
    private bool player_double_attack_chance = false;

    //게임 시스템의 진행을 정지시키기 위한 변수
    public bool isPlaying = true;
    private bool isatkEnded = false;

    public delegate void Pin_Damage(int Damage);
    public event Pin_Damage? Pin_Damage_Event;

    public PlayerAnimatorMobile playerAnimation;

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
    /// stateStarted로 스핀락을 구현하여
    /// 턴을 강제함.
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
                    playerManger.attackAnim();
                    int random_value = UnityEngine.Random.Range(0, 99);
                    if (random_value < playerManger.playerState.Player_DoubleUpChance) player_double_attack_chance = true;
                    stateStarted = true;
                    isatkEnded = false;
                    Debug.Log("Player attacking...");
                }
                if (enemyAtkEnded() && playerManger.doneAtk())
                {
                    stateStarted = false;
                    currentTurn = GameTurn.EnemyBehaviorState;
                }
                break;

            case GameTurn.EnemyBehaviorState:
                if (!stateStarted)
                {
                    //살아있는 적이 없다면 게임 클리어 처리
                    if (enemyListManager.isAllEnemyDead())
                    {
                        stateStarted = true;    //게임 진행을 막기 위해 stateStarted의 값을 참으로
                        isPlaying = false;

                        //게임 클리어 팝업 띄우기
                        clearPopup.SetActive(true);

                    }
                    else   //살아있는 적이 있다면 적의 이동 작동
                    {

                        Debug.Log("Enemies moving...");
                        // 적 이동을 시작 (enemyListManager.MoveEnemies()가 내부적으로 이동을 처리하고,
                        // AllEnemiesMoved()가 이동 완료를 판단한다고 가정)
                        enemyListManager.MoveEnemies();
                        stateStarted = true;
                    }

                }
                if (enemyListManager.AllEnemiesMoved() && isPlaying)
                {
                    Debug.Log($"적이 다 움직임. {enemyListManager.AllEnemiesMoved()}");
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
        //공이 다 사라졌을 시에 실행
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
        // 스테이지 종료 조건 체크 로직 (필요에 따라 수정)
        return true;
    }

    public void updateBuffState()
    {
        return;
    }

    // 투사체가 제거될 때 호출되어 상태를 업데이트
    public void NotifyProjectileDestroyed()
    {
        if (player_double_attack_chance)
        {
            //playerAnimation.TriggerAttack(); // 애니메이션에서 키이벤트로 같이 처리.
            playerManger.attackAnim();
            player_double_attack_chance = false;  // 더블 어택 기회 소진
        }
        else
        {
            isatkEnded = true; // 더 이상 공격할 필요 없음 → 공격 턴 종료
        }
    }

    public void Pin_Damage_Event_Func(int Damage)
    {
        if (Pin_Damage_Event != null) Pin_Damage_Event(Damage); else Debug.Log("실행됩니다");
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
        //플레이어 공격 투사체 생성
        //투사체는 스스로 나아가며 적과 접촉하거나 지정한 범위 밖으로 나가면 스스로 제거
        plAtkObj = Instantiate(prefPlayerAtkProjrctile);
        plAtkObj.transform.position = new Vector3(-2.4f, 4.85f, 0);
        Debug.Log("Player attacking...");
        yield return new WaitUntil(() => enemyAtkEnded());
    }

    private IEnumerator EnemyBehaviorTurn()
    {
        Debug.Log("Enemies moving...");

        // 적을 5칸씩 나누어 이동시키기
        yield return enemyListManager.MoveEnemies();

        yield return new WaitUntil(() => enemyListManager.AllEnemiesMoved());

        // 이동이 끝나면 스폰 처리
        enemyListManager.SpawnEnemyPerTurn();
    }

    private IEnumerator MoveEnemiesCoroutine()
    {
        var moveEnemiesTask = enemyListManager.MoveEnemies(); // MoveEnemies() 실행
        yield return new WaitUntil(() => moveEnemiesTask.IsCompleted); // 완료될 때까지 대기
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
        damageSum = 0;
        interactionArea.init_ball();
        yield return new WaitUntil(() => chkStageEnded());
    }
 
 */
