using UnityEngine;

public class EletricFan : MonoBehaviour
{
    public float pushForce = 10f; // �÷��̾�� �������� ���� ũ��
    private Vector2 pushDirection; // �о�� ����
    private bool isOperating = false;

    void Start()
    {
        // ��ǳ�Ⱑ �ٶ󺸴� �������� �о�� (�⺻������ ������)
        pushDirection = transform.right.normalized;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("��������?");
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� ����.");
            StartFan();
        }
        else
        {
            Debug.Log("�̰���.");

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
            // �÷��̾ ��ǳ�� �������� �о
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
