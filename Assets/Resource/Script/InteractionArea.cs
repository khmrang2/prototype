using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject ballPrefab; // 드롭할 공 프리팹
    public int numberOfBalls = 5; // 떨어뜨릴 공의 개수
    public float spawnOffset = 0.1f; // 공이 조금씩 다른 위치에 생성되도록 하기 위한 오프셋

    private bool isReadyToDrop = false;

    // 손가락이 눌렸을 때 호출
    public void OnPointerDown(PointerEventData eventData)
    {
        isReadyToDrop = true; // 공을 떨어뜨릴 준비 상태
        Debug.Log("Touch down detected - ready to drop ball.");
    }

    // 손가락이 떼어졌을 때 호출
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isReadyToDrop)
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            spawnPosition.z = 0; // Z축을 0으로 설정하여 2D 평면에 공이 생성되도록 함

            // 정해진 개수의 공을 떨어뜨리기
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