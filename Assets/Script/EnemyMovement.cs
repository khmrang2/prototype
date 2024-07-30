using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float movedistance = 0.5f;
    public float chaseDistance = 0.75f;
    public Vector3 objectivePoint = Vector3.zero;
    //���� �ð�
    public int stunCount = 2;
    [SerializeField] private GameObject targetObj = null;
    [SerializeField] private GameObject chasingEffect = null;
    [SerializeField] private GameObject stunnedEffect = null;
    private Transform targetTransform = null;
    private PlayerControl playerControl = null;
    private bool detected = false;
    private int stunLeft =0;
    public bool isDead = false;
    public bool isStunned = false;

    private void Start()
    {
        targetTransform = targetObj.transform;
        playerControl = targetObj.GetComponent<PlayerControl>();
        isDead = false ;
        detected = false ;
        isStunned = false;
        stunLeft = stunCount;
        objectivePoint = Vector3.zero;

    }

    private void Update()
    {
        //�÷��̾ ���������� ����
        if (playerControl.playerMoved)
        {
            if (!isStunned)
            {
                EnemyMove();
            }else
            {
                chasingEffect.SetActive(false);
                stunnedEffect.SetActive(true);
                stunLeft--;
                if(stunLeft == 0)
                {
                    isStunned = false ;
                    stunLeft = stunCount;
                    stunnedEffect.SetActive(false);
                }
            }
            playerControl.playerMoved = false;

        }
    }



    public void EnemyMove() {
        if (detected)
        {
            EnemyChase();
        }
        else
        {
            chasingEffect.SetActive(false);
            RandomMove();
        }
        GameOverCheck();
        DeadAnimation();
        EnemyDetect();
    }


    public void EnemyDetect()
    {
        if(Vector2.Distance(transform.position, targetTransform.position) < chaseDistance && !detected)
        {
            detected = true;
            chasingEffect.SetActive(true);
            objectivePoint = targetObj.transform.position;
            //Debug.Log(objectivePoint);
        }
    }


    public void EnemyChase()
    {
        float distX = objectivePoint.x - this.transform.position.x;
        float distY = objectivePoint.y - this.transform.position.y;

        if (Mathf.Abs(distX) >= Mathf.Abs(distY))
        {
            if (distX > 0 && IsMovable(this.transform.position + Vector3.right * movedistance))
            {
                EnemyMoveRight();
            }
            else if (distX < 0 && IsMovable(this.transform.position + Vector3.left * movedistance))
            {
                EnemyMoveLeft();
            }
            //�¿� �̵��� �����ϸ� ��ֹ��� ���� ���̹Ƿ� ���(�̵��Լ� �۵� X)
        }
        else
        {
            if (distY > 0 && IsMovable(this.transform.position + Vector3.up * movedistance))
            {
                EnemyMoveUp();
            }
            else if (distY < 0 && IsMovable(this.transform.position + Vector3.down * movedistance))
            {
                EnemyMoveDown();
            }
            //���� �̵��� �����ϸ� ��ֹ��� ���� ���̹Ƿ� ���(�̵��Լ� �۵� X)
        }

        ReachedObjectivePoint();
    }






    public void RandomMove()
    {
        int a = Random.Range(1, 5);
        switch(a)
        {
            case 1:
                if (IsMovable(this.transform.position + Vector3.up * movedistance))
                {
                    EnemyMoveUp();
                }
                break;
            case 2:
                if (IsMovable(this.transform.position + Vector3.down * movedistance))
                {
                    EnemyMoveDown();
                }
                break;
            case 3:
                if (IsMovable(this.transform.position + Vector3.left * movedistance))
                {
                    EnemyMoveLeft();
                }
                break;
            case 4:
                if (IsMovable(this.transform.position + Vector3.right * movedistance))
                {
                    EnemyMoveRight();
                }
                break;

        }
    }

    public void ReachedObjectivePoint()
    {
        if(objectivePoint == this.transform.position)
        {
            detected = false;
        }
    }


    public void GameOverCheck()
    {
        if(Vector2.Distance(transform.position, targetTransform.position) == 0){
            PlayerControl temp = targetObj.GetComponent<PlayerControl>();
            temp.isDead = true;
        }
    }


    public void EnemyMoveUp() => Move(Vector3.up);
    public void EnemyMoveDown() => Move(Vector3.down);
    public void EnemyMoveRight() => Move(Vector3.right);
    public void EnemyMoveLeft() => Move(Vector3.left);


    public void Move(Vector3 direction)
    {
        Vector3 pos = this.transform.position + direction * movedistance;
        this.transform.position = pos;
        

    }



    private bool IsMovable(Vector3 targetPosition)
    {
        Vector2 currentPosition2D = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition2D = new Vector2(targetPosition.x, targetPosition.y);
        Vector2 direction = targetPosition2D - currentPosition2D;
        float distance = direction.magnitude;

        int layerMask = LayerMask.GetMask("Obstacle", "Trap", "Goal");

        // ����ĳ��Ʈ�� ����Ͽ� �̵��Ϸ��� ��ġ�� �浹�� �ִ��� �˻�
        RaycastHit2D hit = Physics2D.Raycast(currentPosition2D, direction.normalized, distance, layerMask);

        // ����� �α� �߰�
        //Debug.Log($"Raycast from {currentPosition2D} to {targetPosition2D}, Direction: {direction.normalized}, Distance: {distance}, LayerMask: {layerMask}");

        if (hit.collider != null)
        {
            //Debug.Log($"Hit Collider: {hit.collider.gameObject.name}, Position: {hit.collider.transform.position}");
            ITileAction tileAction = hit.collider.GetComponent<ITileAction>();
            if (tileAction != null)
            {
                //�̵��� ���⿡ �ִ� ������Ʈ�� Ʈ���̶��
                if (hit.collider.gameObject.layer == 8)
                {
                    //�������̸� �ƴϸ� ���� ȸ��
                    if (!detected)
                    {
                        return false;
                    }
                    //�������̸� ���� ����
                    else
                    {
                        isDead = true;
                        return true;
                    }
                }

                return tileAction.OnEnemyEnter(this);

            }
            else
            {
                //�̵��� �� ���� ��� == obstacle
                Debug.Log(" Movement blocked by: " + hit.collider.gameObject.name);
                return false;
            }
        }
        else
        {
            // �浹�� �������� �ʾ��� ���� �̵�
            return true;
        }
    }


    public void DeadAnimation() 
    {
        if (isDead)
        {
            this.gameObject.SetActive(false);
        }
    }

}
