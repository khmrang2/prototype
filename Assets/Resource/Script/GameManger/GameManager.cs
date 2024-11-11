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
    public GameObject prefPlayerAtkProjrctile;
    private GameObject plAtkObj;
    public Transform playerTransform;  // 플레이어의 Transform
    public EnemyListManager enemyListManager;  // EnemyListManager 참조
    public int damageSum = 0;
    public GameTurn currentTurn = GameTurn.DropBallState;

    void Start()
    {
        // 게임 루프 시작
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
        yield return new WaitUntil(() => chooseBuffEnded());
    }

    private IEnumerator EndChkStage()
    {
        Debug.Log("Checking end conditions...");
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
        //투사체 적과 충돌하거나 화면 밖으로 나가면 제거
        //투사체 담긴 변수가 null이 되었다면 플레이어 공격 종료로 판단하고 true 반환
        // 아니면 false 반환

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
        return true;  // 적의 이동이 끝났는지를 체크하는 로직
    }

    private bool spawnEnemyEnded()
    {
        return true;  // 적이 모두 소환됐는지를 체크하는 로직
    }

    private bool chooseBuffEnded()
    {
        return true;  // 버프가 선택됐는지를 체크하는 로직
    }

    private bool chkStageEnded()
    {
        return true;  // 스테이지가 끝났는지를 체크하는 로직
    }
}