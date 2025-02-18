using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveDistance = 10f;  // �̵� �Ÿ�
    private bool hasMoved = false;   // �̵� ���� Ȯ��

    public void Move()
    {
        if (!hasMoved) // �� �Ͽ� �� ���� �̵�
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
        hasMoved = false; // ���� �Ͽ��� �ٽ� �̵� �����ϵ��� ����
    }
}
