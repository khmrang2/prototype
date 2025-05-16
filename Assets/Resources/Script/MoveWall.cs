using UnityEngine;

public class PingPongObstacle2D : MonoBehaviour
{
    public float moveDistance = 3f;  // 좌우 이동 범위 (총 거리)
    public float speed = 2f;         // 이동 속도

    private Vector3 startPosition;

    void Start()
    {
        // 시작 위치를 기준으로 이동
        startPosition = transform.position;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * speed, moveDistance);
        transform.position = startPosition + new Vector3(offset, 0, 0);
    }
}
