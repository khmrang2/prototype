using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject target;  // �̵��� ��ǥ ������Ʈ
    public float moveSpeed = 5f;  // �̵� �ӵ�
    private bool isMoving = false; // �̵� ���� Ȯ��

    private void Start()
    {
        // "Player" �±װ� �ִ� ������Ʈ�� ã�� Ÿ������ ����
        target = GameObject.FindWithTag("Player");
    }

    public void Move()
    {
        if (target != null && !isMoving) // Ÿ���� �����ϰ� �̵� ���� �ƴ� ���� �̵� ����
        {
            isMoving = true;
        }
    }

    private void Update()
    {
        if (isMoving && target != null)
        {
            // Ÿ�� �������� �̵�
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

            // ��ǥ ��ġ�� �����ϸ� �̵� ����
            if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

    public bool HasMoved()
    {
        return isMoving;
    }

    public void ResetMove()
    {
        isMoving = false; // �̵� ���� �ʱ�ȭ
    }
}
