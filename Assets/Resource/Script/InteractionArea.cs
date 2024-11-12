using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject ballPrefab; // ����� �� ������
    private int numberOfBalls = 5; // ����߸� ���� ����
    public float spawnOffset = 0.1f; // ���� ���ݾ� �ٸ� ��ġ�� �����ǵ��� �ϱ� ���� ������
    //public bool isReadyToDrop = false;

    // �հ����� ������ �� ȣ��
    public void OnPointerDown(PointerEventData eventData)
    {
        //isReadyToDrop = true; // ���� ����߸� �غ� ����
        if (numberOfBalls != 0) Debug.Log("Touch down detected - ready to drop ball.");
    }

    // �հ����� �������� �� ȣ��
    public void OnPointerUp(PointerEventData eventData)
    {
        if (numberOfBalls != 0)
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            spawnPosition.z = 0; // Z���� 0���� �����Ͽ� 2D ��鿡 ���� �����ǵ��� ��

            for(; numberOfBalls > 0; numberOfBalls--){
                Vector3 offset = new Vector3(Random.Range(-spawnOffset, spawnOffset), Random.Range(-spawnOffset, spawnOffset), 0);
                Instantiate(ballPrefab, spawnPosition + offset, Quaternion.identity);
            }
            Debug.Log("Touch released - ball dropped."+get_ball_num());
        }
    }

    public void init_ball(){
        numberOfBalls = 5;
    }
    public int get_ball_num(){
        return numberOfBalls;
    }
}