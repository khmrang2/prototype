using UnityEngine;

public interface IItemPickup
{
    // �÷��̾ �����۰� �浹�� �� ȣ��˴ϴ�.
    void OnPlayerCollider2DEnter(Collider2D playerCollider);

    // �÷��̾ �����۰��� �浹�� ������ �� ȣ��˴ϴ�.
    void OnPlayerCollider2DExit(Collider2D playerCollider);
}
