using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임의 각 상황을 정의하고 제어하기 위해서
// 그냥 이름 지은 enum.
public enum GameTurn
{
    DropBall,
    EnemyMove,
    ChooseBuff,
}


// 버프 상황을 제어하기 위해 만든 구조체.
struct buffState
{
    int numberOfBalls;
    //int spawnOffset;
    int damageOfBall;
};

public class GameManager : MonoBehaviour
{
    public List<Ball> Balls; 
    public GameTurn currentTurn = GameTurn.DropBall;

    void Start()
    {
        // 게임을 시작할 프레임 워크의 시작.
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            switch (currentTurn)
            {
                case GameTurn.DropBall:
                    yield return StartCoroutine(DropBallTurn());
                    currentTurn = GameTurn.EnemyMove;
                    break;
                case GameTurn.EnemyMove:
                    yield return StartCoroutine(EnemyMoveTurn());
                    currentTurn = GameTurn.ChooseBuff;
                    break;
                case GameTurn.ChooseBuff:
                    yield return StartCoroutine(ChooseBuffTurn());
                    currentTurn = GameTurn.DropBall;
                    break;
            }
        }
    }

    private IEnumerator DropBallTurn()
    {
        Debug.Log("플레이어 턴(드랍 볼 루틴)");
        // Drop the ball logic here
        // Wait for the user to interact (e.g., ball dropping completion)
        yield return new WaitUntil(() => BallHasDropped()); // Replace with actual condition
    }

    private IEnumerator EnemyMoveTurn()
    {
        Debug.Log("적의 턴");
        // Logic to move the enemy here
        yield return new WaitForSeconds(1); // Replace with actual enemy movement logic
    }

    private IEnumerator ChooseBuffTurn()
    {
        Debug.Log("");
        // Logic for choosing a buff (e.g., wait for user input)
        yield return new WaitUntil(() => BuffChosen()); // Replace with actual condition
    }

    private bool BallHasDropped()
    {
        //
        return true; // Replace with actual check
    }

    private bool BuffChosen()
    {
        // Implement condition to check if a buff has been chosen
        return true; // Replace with actual check
    }
}