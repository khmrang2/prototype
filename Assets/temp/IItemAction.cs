using UnityEngine;

public interface IItemPickup
{
    // 플레이어가 아이템과 충돌할 때 호출됩니다.
    void OnPlayerCollider2DEnter(Collider2D playerCollider);

    // 플레이어가 아이템과의 충돌을 끝냈을 때 호출됩니다.
    void OnPlayerCollider2DExit(Collider2D playerCollider);
}
