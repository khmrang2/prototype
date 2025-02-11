using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryArea : MonoBehaviour
{
    void Start()
    {
        // 메인 카메라를 기준으로 화면 하단의 좌표를 가져옴
        Vector3 bottomCenterScreen = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 0));

        // Destroy Zone의 위치를 카메라 하단부로 설정
        transform.position = new Vector3(bottomCenterScreen.x, bottomCenterScreen.y - 1.0f, 0); // Y좌표를 살짝 낮춰 카메라에 보이지 않도록 설정

        // Collider2D 크기 조정 (가로 너비 설정)
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.size = new Vector2(Camera.main.orthographicSize * 3 * Camera.main.aspect, 1); // 화면 너비에 맞춤
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 트리거에 들어온 오브젝트가 "Ball" 태그를 가진 경우 제거
        if (other.CompareTag("Ball"))
        {
            Destroy(other.gameObject);
        }
    }
}
