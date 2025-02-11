using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryArea : MonoBehaviour
{
    void Start()
    {
        // ���� ī�޶� �������� ȭ�� �ϴ��� ��ǥ�� ������
        Vector3 bottomCenterScreen = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 0));

        // Destroy Zone�� ��ġ�� ī�޶� �ϴܺη� ����
        transform.position = new Vector3(bottomCenterScreen.x, bottomCenterScreen.y - 1.0f, 0); // Y��ǥ�� ��¦ ���� ī�޶� ������ �ʵ��� ����

        // Collider2D ũ�� ���� (���� �ʺ� ����)
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.size = new Vector2(Camera.main.orthographicSize * 3 * Camera.main.aspect, 1); // ȭ�� �ʺ� ����
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ʈ���ſ� ���� ������Ʈ�� "Ball" �±׸� ���� ��� ����
        if (other.CompareTag("Ball"))
        {
            Destroy(other.gameObject);
        }
    }
}
