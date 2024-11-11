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
public class GameManager : MonoBehaviour
{
    // 현민 - 
    // 게임할 state들을 불러옴.
    // 플레이어가 기본적으로 불러오는 state. 
    // 버프를 받아서 갱신될 cur_state.
    BuffState curState = null;
    [SerializeField]
    public BuffManager buffManager;

    public int damageSum = 0;
    //주석처리한 코드는 써도 되고 안써도되고..
    //public List<Ball> Balls; 
    public GameTurn currentTurn = GameTurn.DropBallState;

    void Start()
    {
        curState = new BuffState();
        // 게임을 시작할 프레임 워크의 시작.
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
        buffManager.ShowBuffSelection(); // 버프 선택 UI 표시

        // 버프가 선택될 때까지 대기
        yield return new WaitUntil(() => buffManager.IsBuffSelected());

        // 선택된 Buff ID 출력 (필요에 따라 추가 작업 수행 가능)
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
        // 이제 여기서 공이 모두 떨어졌는지를 체크하여 return 해주면 됩니다.
        // balls[]라는 리스트를 구현해서 모든 볼이 사라지면 이 BallHasDropped를 invoke해주거나
        // 아니면 마지막 공이 사라진다면 이 코드를 invoke 하는 형식으로 구현해주시면 됩니다.
        return true;
    }
    private bool enemyAtkEnded()
    {
        // 이하동문 
        return true;
    }

    private bool buffChosen()
    {
        return true;
    }

    private bool enemyMoveEnded()
    {
        // 이하동문 
        return true;
    }
    private bool spawnEnemyEnded()
    {
        // 이하동문 
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
        // 이하동문 
        return true;
    }
}