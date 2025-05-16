using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[System.Serializable]
public class Ball : MonoBehaviour
{
    private PlayerState playerState;
    // 버프 : 분열할 공의 구분을 짓기 위함.
    public bool wasSplited = false;

    [SerializeField] private Transform ball;
    [SerializeField] private PhysicsMaterial2D ball_elacity;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Destroy(gameObject, 10f);
    }

    void Awake()
    {
        if (playerState == null)
        {
            playerState = FindObjectOfType<PlayerState>();
        }
        // 생성되었을때 스케일 버프 적용.
        ball.localScale = ball.localScale * playerState.Ball_Size;
        // 생성되었을때 탄성 버프 적용.
        ball_elacity.bounciness = playerState.Ball_Elasticity;
    }

    public void HandlePinCollision(Collision2D collision)
    {
        if (wasSplited || playerState.Ball_Split <= 0) {
            return; // 분열 및 오리지널 공이여야 들어감.
        }

        // 현재 공의 속도 정보 가져오기
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) return;

        Vector3 velocityDir = rb.velocity.normalized; // 속도 방향 벡터

        // 랜덤하게 +30도 또는 -30도 회전
        float angleOffset = Random.Range(0, 2) == 0 ? 15f : -15f;
        Vector3 rotatedDirection = Quaternion.Euler(0, 0, angleOffset) * velocityDir;

        // 충돌 위치에서 1f 떨어진 곳에 생성
        Vector3 spawnPosition = (Vector3)collision.contacts[0].point + rotatedDirection * 1f;

        // 속도 크기 유지
        float speed = rb.velocity.magnitude;

        
        // 분열할 공의 수만큼 공을 분열.
        for (int splitnum = playerState.Ball_Split; splitnum > 0; splitnum--)
        {
            GameObject newBall = Instantiate(gameObject, spawnPosition, Quaternion.identity);
            newBall.GetComponent<Ball>().wasSplited = true;
            Rigidbody2D newRb = newBall.GetComponent<Rigidbody2D>();
            if (newRb != null)
            {
                newRb.velocity = velocityDir * speed; // 기존 공과 동일한 속도 적용
            }
        }
    }
}
