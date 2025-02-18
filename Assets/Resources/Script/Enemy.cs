using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveDistance = 10f;  // 이동 거리
    private bool hasMoved = false;   // 이동 여부 확인

    public void Move()
    {
        if (!hasMoved) // 한 턴에 한 번만 이동
        {
            transform.position += Vector3.left * moveDistance;
            hasMoved = true;
        }
    }

    public bool HasMoved()
    {
        return hasMoved;
    }

    public void ResetMove()
    {
        hasMoved = false; // 다음 턴에서 다시 이동 가능하도록 리셋
    }
}
