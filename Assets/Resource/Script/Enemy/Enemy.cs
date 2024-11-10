using UnityEngine;
public class Enemy : MonoBehaviour
{
    public int damage = 10;         // 적의 공격력
    public float attackRange = 5f;  // 공격 사정거리
    public float moveDistance = 2f; // 이동 거리

    // 플레이어가 공격 사정거리에 있는지 확인
    public bool IsPlayerInRange(Transform player)
    {
        float distance = Vector3.Distance(transform.position, player.position);
        return distance <= attackRange;
    }

    // 플레이어를 공격
    public void AttackPlayer(Transform player)
    {
        Debug.Log("Attacking player with damage: " + damage);
        // 플레이어에게 데미지 가하는 로직 구현
    }

    // 플레이어 쪽으로 이동
    public void MoveTowardsPlayer(Transform player)
    {
        Debug.Log("Moving towards player...");
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveDistance;
    }
}
