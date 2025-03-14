using System.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;  // �̵� �ӵ�
    [SerializeField] public bool isMoving = false; // �̵� ���� Ȯ��
    public bool isDetectedPlayer = false;   //�÷��̾� ���� ����
    private PlayerManger Pmanager;  //�÷��̾�� ������ ó���� ���� PlayerManager
    private EnemyStatus status; //�� �� �ɸ����� ����
    public bool isSpawned = false;
    public bool isAlive;
    [SerializeField] private float AttackRange = 0.1f;
    private Vector3 targetPosition;  // ��ǥ ��ġ
    private int moveStep = 0; // �̵� �ܰ� (5������ ������ ����);
    private GameObject target;  // �߰��� ����, Ÿ��(�÷��̾�)�� ����
    float moveDistance; // �� ĭ �̵� ��ǥ �Ÿ�
    private void Start()
    {
        
        status = GetComponent<EnemyStatus>();
        // "Player" �±װ� �ִ� ������Ʈ�� ã�� Ÿ������ ����
        target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            //ã������ PlayerManager �޾ƿ���
            Pmanager = target.GetComponent<PlayerManger>();

            //����׿�
            if (Pmanager != null)
            {
                Debug.Log("found player!");
            }
        }
        moveDistance = Vector3.Distance(transform.position, target.transform.position) / 5f; // �� ĭ �̵� ��ǥ �Ÿ�
        //�÷��̾� ���� ���¸� false�� �ʱ�ȭ
        isDetectedPlayer = false;
        //���� ó���� true��
        isAlive = true;
    }

    public async Task Move()
    {
        if (target != null && !isMoving) // Ÿ���� �����ϰ� ��ȯ�� �Ϸ�Ǿ����� �̵� ���� �ƴ� ���� �̵� ����
        {
            if (isSpawned)
            {
                //��Ÿ� ���� �÷��̾ �����ϸ�
                if (isDetectedPlayer)
                {
                    await Attack(); //������ ���� ������ ���
                    isMoving = false;   //�̵��� ���ʿ��ϹǷ� false
                }
                else
                {
                    isMoving = true;    //��Ÿ� ���� ���� ���;� �ϴ� �̵�
                    moveStep++;         // �̵� �ܰ踦 �ϳ� ����
                    if (moveStep >= 5)  // 5�ϸ��� �� ĭ �̵�
                    {
                        await MoveOneStep(); // �� ĭ �̵�
                        moveStep = 0; // �̵� �ܰ踦 �ʱ�ȭ
                    }
                }
            }
            else
            {
                //���� ó���� �ȵǾ��ٸ� �̵��ϸ� �ȵǴ� false
                isMoving = false;
            }
        }
    }
    
    private float duration = 0.2f; // �̵� �ð�
    public async Task MoveOneStep()
    {
        if (target == null || !isSpawned) return;
        isMoving = true; // �̵� ����
        float elapsedTime = 0f;
        
        
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position - (Vector3.right * moveDistance);

        while (elapsedTime < duration)
        {
            // ��ǥ ��ġ�� ���� �ӵ��� �̵�
            Debug.Log($"{Time.deltaTime} �ð�");
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            await Task.Yield(); // �����Ӹ��� ����
        }
        transform.position = endPosition; // ���� ��ġ ����
        isMoving = false; // �̵� ����

        DetectPlayer(); // �̵� �� �÷��̾� ����
    }

    private void Update()
    {
        if (!isMoving) return; // �̵� ���� �ƴ� ���� ����

        // ��Ÿ� ���� �÷��̾ �ִ��� Ȯ��
        DetectPlayer();
    }

    public bool HasMoved()
    {
        return !isMoving;  // �̵��� �����ٸ� false ��ȯ
    }

    public void ResetMove()
    {
        isMoving = false; // �̵� ���� �ʱ�ȭ
    }

    //�÷��̾ ��Ÿ� ���� �����ϴ��� Ȯ��
    private void DetectPlayer()
    {
        if (Vector3.Distance(transform.position, target.transform.position) <= AttackRange)
        {
            isDetectedPlayer = true;
        }
    }

    //���� �Լ�
    private async Task Attack()
    {
        // �÷��̾�� ���ݷ¸�ŭ ������ �ο�
        Pmanager.playerHP -= status.EnemyATK;

        // �ӽ� �ڵ� (���� �ִϸ��̼��� ���� ��� �ð�)
        await Task.Delay(500);  // 0.5�� ���
    }

}
    //���� �Լ����� ���ϸ��̼� ���̸� ��ȯ�ޱ� ���� ���̴� ����
    //private float GetAnimationLength(string animationName)
    //{
    //    if (animator == null) return 1.0f; // �ִϸ����� ���� ��� �⺻��

    //    //���ϸ��̼��� �̷�� Ŭ������ ���̸� �����ϱ� ���� RuntimeAnimatorController
    //    RuntimeAnimatorController ac = animator.runtimeAnimatorController;

    //    foreach (AnimationClip clip in ac.animationClips)
    //    {
    //        //���ϸ��̼��� �̷�� �� Ŭ������
    //        if (clip.name == animationName)
    //            return clip.length;
    //    }
    //    return 1.0f; // �⺻ �ִϸ��̼� ���� (1��)
    //}