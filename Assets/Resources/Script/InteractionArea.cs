using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerState playerState;

    public GameObject ballPrefab; // 공 프리팹
    private int numberOfBalls = 3; // 디폴트 3 : PlayerState로 변경.
    public float spawnOffset = 0.1f; // 누른 위치에서 offset만큼 떨어진 거리에서 공 스폰.
    //public bool isReadyToDrop = false;

    void Start()
    {
        numberOfBalls = playerState.Ball_Count;
    }

    // 눌렀을때
    public void OnPointerDown(PointerEventData eventData)
    {
        //isReadyToDrop = true; // ���� ����߸� �غ� ����
        if (numberOfBalls != 0) Debug.Log("Touch down detected - ready to drop ball.");
    }

    // 누르고 뗐을때 공이 떨어짐.
    public void OnPointerUp(PointerEventData eventData)
    {
        if (numberOfBalls != 0)
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            spawnPosition.z = 0; // z축 초기화,.
            GameObject newBall = null;
            for(; numberOfBalls > 0; numberOfBalls--){
                Vector3 offset = new Vector3(Random.Range(-spawnOffset, spawnOffset), Random.Range(-spawnOffset, spawnOffset), 0);
                newBall = Instantiate(ballPrefab, spawnPosition + offset, Quaternion.identity);
                // 버프 : 분열할 공의 구분을 짓기 위함.
                newBall.GetComponent<Ball>().wasSplited = false;
            }
            Debug.Log("Touch released - ball dropped."+get_ball_num());
        }
    }

    public void init_ball(){
        numberOfBalls = playerState.Ball_Count;
    }
    public int get_ball_num(){
        return numberOfBalls;
    }
}