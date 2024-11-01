using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �� ��Ȳ�� �����ϰ� �����ϱ� ���ؼ�
// �׳� �̸� ���� enum.
public enum GameTurn
{
    DropBall,
    EnemyMove,
    ChooseBuff,
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
    public List<Ball> Balls; 
    public GameTurn currentTurn = GameTurn.DropBall;

    void Start()
    {
        // ������ ������ ������ ��ũ�� ����.
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
        Debug.Log("�÷��̾� ��(��� �� ��ƾ)");
        // Drop the ball logic here
        // Wait for the user to interact (e.g., ball dropping completion)
        yield return new WaitUntil(() => BallHasDropped()); // Replace with actual condition
    }

    private IEnumerator EnemyMoveTurn()
    {
        Debug.Log("���� ��");
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