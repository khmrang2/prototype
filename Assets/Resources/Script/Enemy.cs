using UnityEngine;
public class Enemy : MonoBehaviour
{
    public int damage = 10;
    public float attackRange = 5f;
    public float moveSpeed = 5f;

    public void Act(Transform playerTransform)
    {
        // 플레이어 위치로 이동
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * moveSpeed;
    }

    private void AttackPlayer()
    {
        Debug.Log("Attacking the player with damage: " + damage);
        // Implement player damage logic here (e.g., reduce player's health)
    }

    private void MoveTowardsPlayer(Transform playerTransform)
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
