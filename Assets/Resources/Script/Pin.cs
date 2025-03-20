using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    // 파티클 생성
    public GameObject particlePrefab; // 파티클 프리팹
    public float particleLifetime = 2f; // 파티클 유지 시간

    private float targetRotation; // 목표 각도
    private float rotationSpeed = 200f; // 기본 회전 속도 (각도/초)
    private float maxRotationSpeed = 20f; // 최소 회전 속도
    private float minRotationSpeed = 400f; // 최대 회전 속도 (각도/초)

    private float forceOffset = 15.0f;

    private int cnt = 0;
    private int maxHitCount = 5; // 최대 충돌 횟수

    private void Start()
    {
        // 초기 목표 각도를 현재 각도로 설정
        targetRotation = transform.eulerAngles.z;
    }

    // 충돌
    private void OnCollisionEnter2D(Collision2D collision)
    {

        // 충돌한 오브젝트의 태그가 "ball"인지 확인
        if (collision.gameObject.CompareTag("Ball"))
        {
            // 충돌 점
            ContactPoint2D contactPoint = collision.contacts[0];

            // 충돌한 공의 스크립트를 불러와서 공을 분열시킴.
            Ball ballScript = collision.gameObject.GetComponent<Ball>();
            if (ballScript != null)
            {
                ballScript.HandlePinCollision(collision);
                ballScript.wasSplited = true;
            }

            // 충돌의 힘 등을 계산해야 하기에
            temp(collision);
            makeSteamEffect(contactPoint.point);

            // add_cnt 함수 호출
            add_cnt();
            
            destroy_Pin();
        }
    }

    // 1. 핀과 공이 부딪히는 상대위치를 파악하여 좌/우로 회전하는 기능
    // -> 내부에 각을 계산하여, 시간과 점진적으로 회전하게 만듬.
    // 2. 핀과 공이 부딪히는 힘을 가지고 회전하는 각을 변화시킴.
    // ->  큰  힘 : 더 많은 각 + 짧은 시간,
    // -> 작은 힘 : 작은 각 + 긴 시간.
    // => collision.relativeVelocity.magnitude 가 1~7로 대강 찍힘. 상수로 19를 곱함.
    // 3. (2)번에 핀과의 충돌위치(0도면 힘이 거의 안전해지고, 90도면 힘이 풀로 전해지는)도 곱함. 
    // -> 삼각함수의 sin 함수를 이용하여 
    private void temp(Collision2D collision)
    {
        // 1 : 충돌 지점 가져와서
        ContactPoint2D contactPoint = collision.contacts[0];
        // 2 : 충돌 힘 계산,
        float collisionForce = collision.relativeVelocity.magnitude; // 충돌 속도의 크기
                                                                     //Debug.Log("collision Force is : " + collisionForce + "\n");
                                                                     // 1 : 충돌 지점이 객체의 중심 좌표를 기준으로 왼/오 판단.
        Vector2 collisionDirection = contactPoint.point - (Vector2)transform.position;

        float relativeAngle = Vector2.SignedAngle(Vector2.up, collisionDirection);
        float angleWeight = CalculateAngleWeight(relativeAngle);

        // 충돌 방향에 따라 회전 각도 설정 -> 1 ~ 9
        float rotationAmount = Mathf.Clamp(collisionForce * angleWeight * forceOffset, 0f, 180f); // 0 < n < 180
        //Debug.Log("rotationAmount : " + rotationAmount + "\n");

        if (collisionDirection.x < 0) // 왼쪽 충돌
        {
            targetRotation += rotationAmount;
        }
        else if (collisionDirection.x > 0) // 오른쪽 충돌
        {
            targetRotation -= rotationAmount;
        }

        // 목표 각도를 0~360도로 정규화
        targetRotation = NormalizeAngle(targetRotation);

        // 회전속도 계산
        rotationSpeed = Mathf.Clamp(collisionForce * 50f, minRotationSpeed, maxRotationSpeed);
    }

    private void Update()
    {
        // 현재 각도를 목표 각도로 점진적으로 이동
        float currentRotation = transform.eulerAngles.z;
        float newRotation = Mathf.MoveTowardsAngle(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, newRotation);
    }

    // 1 : 각도를 0~360도로 정규화
    private float NormalizeAngle(float angle)
    {
        while (angle < 0) angle += 360f;
        while (angle >= 360f) angle -= 360f;
        return angle;
    }

    // 3 : 상대 각도에 따른 가중치 계산 함수 (사인 함수 기반)
    private float CalculateAngleWeight(float relativeAngle)
    {
        // -180~180도 범위의 각도를 0~180도로 변환
        relativeAngle = Mathf.Abs(relativeAngle);

        // 사인 함수를 이용해 가중치 계산 (0~180도: 0~1)
        return Mathf.Sin(relativeAngle * Mathf.Deg2Rad);
    }

    private void makeSteamEffect(Vector2 colligionPoint)
    {
        // 파티클 생성
        GameObject particleInstance = Instantiate(particlePrefab, colligionPoint, Quaternion.identity);

        // 파티클 자동 삭제
        Destroy(particleInstance, particleLifetime);
    }
    private void destroy_Pin() // 핀이 일정 체력 이하로 떨어지면 파괴
    {
        if (cnt >= maxHitCount) gameObject.SetActive(false);
    }
    private void add_cnt(){
        cnt++;
    }

    public int hit_cnt(){
        return cnt;
    }
    public void init_cnt(){
        cnt = 0;
    }
}
