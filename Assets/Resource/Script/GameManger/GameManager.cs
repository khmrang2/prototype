using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct buffState
{
    int numberOfBalls;
    //int spawnOffset;
    int damageOfBall;
};

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab; // 드롭할 공 프리팹
    public int numberOfBalls = 5; // 떨어뜨릴 공의 개수
    public float spawnOffset = 0.1f; // 공이 조금씩 다른 위치에 생성되도록 하기 위한 오프셋

    private int damage = 0;

    void Update()
    {
        // 화면을 터치했는지 확인
        /*
         * 플레이어가 화면을 터치한 부분의 좌표를 받습니다.
         * 해당 위치에 공의 개수 만큼 Instantiate로 받은 위치에 ball prefab을 소환합니다..
         * ball 프리팹에 대한 것은 이미 만들어 놓았으니(Resource/Prefabs에 위치함. 물리 머터리얼, 리지드바디 등 다 적용되어있음)
         */
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPosition.z = 0; // Z축을 0으로 설정하여 2D 평면에 공이 생성되도록 함

            // 정해진 개수의 공을 떨어뜨리기
            for (int i = 0; i < numberOfBalls; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-spawnOffset, spawnOffset), Random.Range(-spawnOffset, spawnOffset), 0);
                Instantiate(ballPrefab, spawnPosition + offset, Quaternion.identity);
            }
        }
    }

    //void (Vector3 textPrintPoint)
    //{

    //}
}
