using UnityEngine;

public class EletricFan : MonoBehaviour
{
    public float pushForce = 10f; // 플레이어에게 가해지는 힘의 크기
    private Vector2 pushDirection; // 밀어내는 방향
    private bool isOperating = false;

    void Start()
    {
        // 선풍기가 바라보는 방향으로 밀어내기 (기본적으로 오른쪽)
        pushDirection = transform.right.normalized;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("실행은됨?");
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("플레이어 감지.");
            StartFan();
        }
        else
        {
            Debug.Log("미감지.");

        }
    }

    void StartFan()
    {
        isOperating = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (isOperating && other.CompareTag("Player"))
        {
            // 플레이어를 선풍기 방향으로 밀어냄
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1f, 0);
            if (playerRb != null)
            {
                playerRb.velocity = pushDirection * pushForce;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isOperating = false;
        }
    }
}
