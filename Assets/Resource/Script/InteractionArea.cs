using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject ballPrefab; // ����� �� ������
    public int numberOfBalls = 5; // ����߸� ���� ����
    public float spawnOffset = 0.1f; // ���� ���ݾ� �ٸ� ��ġ�� �����ǵ��� �ϱ� ���� ������

    private bool isReadyToDrop = false;

    // �հ����� ������ �� ȣ��
    public void OnPointerDown(PointerEventData eventData)
    {
        isReadyToDrop = true; // ���� ����߸� �غ� ����
        Debug.Log("Touch down detected - ready to drop ball.");
    }

    // �հ����� �������� �� ȣ��
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isReadyToDrop)
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            spawnPosition.z = 0; // Z���� 0���� �����Ͽ� 2D ��鿡 ���� �����ǵ��� ��

            // ������ ������ ���� ����߸���
            for (int i = 0; i < numberOfBalls; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-spawnOffset, spawnOffset), Random.Range(-spawnOffset, spawnOffset), 0);
                Instantiate(ballPrefab, spawnPosition + offset, Quaternion.identity);
            }

            isReadyToDrop = false;
            Debug.Log("Touch released - ball dropped.");
        }
    }
}