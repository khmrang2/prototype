using UnityEngine;
public class Enemy : MonoBehaviour
{
    public int damage = 10;         // ���� ���ݷ�
    public float attackRange = 5f;  // ���� �����Ÿ�
    public float moveDistance = 2f; // �̵� �Ÿ�

    // �÷��̾ ���� �����Ÿ��� �ִ��� Ȯ��
    public bool IsPlayerInRange(Transform player)
    {
        float distance = Vector3.Distance(transform.position, player.position);
        return distance <= attackRange;
    }

    // �÷��̾ ����
    public void AttackPlayer(Transform player)
    {
        Debug.Log("Attacking player with damage: " + damage);
        // �÷��̾�� ������ ���ϴ� ���� ����
    }

    // �÷��̾� ������ �̵�
    public void MoveTowardsPlayer(Transform player)
    {
        Debug.Log("Moving towards player...");
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveDistance;
    }
}
