using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // ��Ʈ ���������� �� movable.
    public bool movable = false;
    //��� ������ ����
    public bool isDead = false;
    //������ ��ȣ ���޿� ����
    public bool playerMoved = false;
    public float moveDistance = 1.0f;
    public Vector3 targetPosition;
    [SerializeField] public GameObject[] enemyList = null;
    public GameObject sonaPrefab = null;
    private GameObject sona = null;


    private void Start()
    {
        isDead = false ;
        movable = false ;
        playerMoved = false ;
    }

    private void Update()
    {
        //������ ������ ��� ���ӿ����� �߱� ���� update�� ���ӿ��� �Լ� ��ġ��Ŵ
        if (isDead)
        {
            //���� ���� ȿ���� ���� �ӽ� �ڵ�
            Debug.Log("Game Over!");
            Time.timeScale = 0.0f;
            this.gameObject.SetActive(false);
        }
    }

    public void MoveUp() => Move(Vector3.up);
    public void MoveDown() => Move(Vector3.down);
    public void MoveRight() => Move(Vector3.right);
    public void MoveLeft() => Move(Vector3.left);


    public void Move(Vector3 direction)
    {
        if(sona !=null)
        {
            Destroy(sona);
        }
        Vector3 pos = this.transform.position + direction * moveDistance;
        if (movable && HandleMove(pos))
        {
            this.transform.position = pos;
            movable = false;
            sona = Instantiate(sonaPrefab, this.transform.position, Quaternion.identity);
        }
        playerMoved = true ;
        
    }

    private bool HandleMove(Vector3 targetPosition)
    {
        Vector2 currentPosition2D = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition2D = new Vector2(targetPosition.x, targetPosition.y);
        Vector2 direction = targetPosition2D - currentPosition2D;
        float distance = direction.magnitude;

        int layerMask = LayerMask.GetMask("Obstacle", "Trap", "Goal", "Enemy");

        // ����ĳ��Ʈ�� ����Ͽ� �̵��Ϸ��� ��ġ�� �浹�� �ִ��� �˻�
        RaycastHit2D hit = Physics2D.Raycast(currentPosition2D, direction.normalized, distance, layerMask);

        // ����� �α� �߰�
        //Debug.Log($"Raycast from {currentPosition2D} to {targetPosition2D}, Direction: {direction.normalized}, Distance: {distance}, LayerMask: {layerMask}");

        if (hit.collider != null)
        {
            //Debug.Log( $"Hit Collider: {hit.collider.gameObject.name}, Position: {hit.collider.transform.position}");
            ITileAction tileAction = hit.collider.GetComponent<ITileAction>();
            if (tileAction != null)
            {
                //�̵��� ���⿡ �ִ� ������Ʈ�� Ʈ���̶��
                if (hit.collider.gameObject.layer == 8)
                {
                    isDead = true;
                }
                //... ���̶��
                else if (hit.collider.gameObject.layer == 7) 
                {
                    
                }
                //... ���̶��
                else if(hit.collider.gameObject.layer== 9)
                {
                    StunEnemy(targetPosition);
                    return false;
                }

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


    public void StunEnemy(Vector3 targrtPos)
    {
        for(int i = 0; enemyList.Length > i; i++)
        {
            if (enemyList[i].transform.position == targrtPos)
            {
                EnemyMovement em = enemyList[i].GetComponent<EnemyMovement>();
                em.isStunned = true;
            }
        }
    }

    public bool gamecheck() { return isDead; }
}
