using UnityEngine;

public class DestoryArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 트리거에 들어온 오브젝트가 "Ball" 태그를 가진 경우 제거
        if (other.CompareTag("Ball"))
        {
            Destroy(other.gameObject);
        }
    }
}