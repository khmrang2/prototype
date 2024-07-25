using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // ��Ʈ ���������� �� movable.
    public bool movable = false;
    public float moveDistance = 1.0f;
    public Vector3 targetPosition;
    [SerializeField] private GameObject Enemy = null;
    EnemyMovement enemyCtr = null;

    private void Start()
    {
        enemyCtr = Enemy.GetComponent<EnemyMovement>();
    }

    public void MoveUp() => Move(Vector3.up);
    public void MoveDown() => Move(Vector3.down);
    public void MoveRight() => Move(Vector3.right);
    public void MoveLeft() => Move(Vector3.left);


    public void Move(Vector3 direction)
    {
        Vector3 pos = this.transform.position + direction * moveDistance;
        if (movable && HandleMove(pos))
        {
            this.transform.position = pos;
            movable = false;
        }
        enemyCtr.EnemyMove();
    }

    private bool HandleMove(Vector3 targetPosition)
    {
        Vector2 currentPosition2D = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition2D = new Vector2(targetPosition.x, targetPosition.y);
        Vector2 direction = targetPosition2D - currentPosition2D;
        float distance = direction.magnitude;

        int layerMask = LayerMask.GetMask("Obstacle", "Trap", "Goal");

        // ����ĳ��Ʈ�� ����Ͽ� �̵��Ϸ��� ��ġ�� �浹�� �ִ��� �˻�
        RaycastHit2D hit = Physics2D.Raycast(currentPosition2D, direction.normalized, distance, layerMask);

        // ����� �α� �߰�
        Debug.Log($"Raycast from {currentPosition2D} to {targetPosition2D}, Direction: {direction.normalized}, Distance: {distance}, LayerMask: {layerMask}");

        if (hit.collider != null)
        {
            Debug.Log( $"Hit Collider: {hit.collider.gameObject.name}, Position: {hit.collider.transform.position}");
            ITileAction tileAction = hit.collider.GetComponent<ITileAction>();
            if (tileAction != null)
            {
                return tileAction.OnPlayerEnter(this);
            }
            else
            {
                // �÷��̾ �̵��� �� ���� ��� == obstacle
                Debug.Log(" Movement blocked by: " + hit.collider.gameObject.name);
                return false;
            }
        }
        else
        {
            // �浹�� �������� �ʾ��� ���� �̵�
            transform.position = targetPosition;
            return true;
        }
    }


}
