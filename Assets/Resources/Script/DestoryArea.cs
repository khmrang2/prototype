using UnityEngine;

public class DestoryArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ʈ���ſ� ���� ������Ʈ�� "Ball" �±׸� ���� ��� ����
        if (other.CompareTag("Ball"))
        {
            Destroy(other.gameObject);
        }
    }
}