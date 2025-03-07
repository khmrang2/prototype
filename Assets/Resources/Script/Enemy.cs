using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject target;  // 이동할 목표 오브젝트
    public float moveSpeed = 5f;  // 이동 속도
    private bool isMoving = false; // 이동 여부 확인

    private void Start()
    {
        // "Player" 태그가 있는 오브젝트를 찾아 타겟으로 설정
        target = GameObject.FindWithTag("Player");
    }

    public void Move()
    {
        if (target != null && !isMoving) // 타겟이 존재하고 이동 중이 아닐 때만 이동 시작
        {
            isMoving = true;
        }
    }

    private void Update()
    {
        if (isMoving && target != null)
        {
            // 타겟 방향으로 이동
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

            // 목표 위치에 도달하면 이동 종료
            if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

    public bool HasMoved()
    {
        return isMoving;
    }

    public void ResetMove()
    {
        isMoving = false; // 이동 상태 초기화
    }
}
