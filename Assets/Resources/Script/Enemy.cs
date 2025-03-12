using System.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject target;  // �̵��� ��ǥ ������Ʈ
    public float moveSpeed = 5f;  // �̵� �ӵ�
    private bool isMoving = false; // �̵� ���� Ȯ��
    public bool isDetectedPlayer = false;   //�÷��̾� ���� ����
    private PlayerManger Pmanager;  //�÷��̾�� ������ ó���� ���� PlayerManager
    private EnemyStatus status; //�� �� �ɸ����� ����
    //private Animator animator; // �ִϸ�����(���� ���ϸ��̼� �߰� �� ���� ����)

    [SerializeField] private float AttackRange = 0.1f;
    
    
    private void Start()
    {
        status = GetComponent<EnemyStatus>();
        // "Player" �±װ� �ִ� ������Ʈ�� ã�� Ÿ������ ����
        target = GameObject.FindWithTag("Player");

        if(target != null)
        {
            //ã������ PlayerManger �޾ƿ���
            Pmanager = target.GetComponent<PlayerManger>();

            //����׿�
            if(Pmanager != null)
            {
                Debug.Log("found player!");
            }
        }

        //�÷��̾� ���� ���¸� false�� �ʱ�ȭ
        isDetectedPlayer = false;

        //���� ���� ���ϸ��̼� �߰� �� ���� �뵵
        //animator = GetComponent<Animator>();
    }

    public async Task Move()
    {
        if (target != null && !isMoving) // Ÿ���� �����ϰ� �̵� ���� �ƴ� ���� �̵� ����
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
            }
        }
    }

    private void Update()
    {
        if (isMoving && target != null)
        {
            // Ÿ�� �������� �̵�
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

            // ��ǥ ��ġ�� �����ϸ� �̵� ����
            if (Vector3.Distance(transform.position, target.transform.position) < AttackRange)
            {
                isMoving = false;
            }
            //��Ÿ� ���� �÷��̾ �ִ��� Ȯ��
            DetectPlayer();
        }
    }

    public bool HasMoved()
    {
        return isMoving;
    }

    public void ResetMove()
    {
        isMoving = false; // �̵� ���� �ʱ�ȭ
    }


    //�÷��̾ ��Ÿ� ���� �����ϴ��� Ȯ�� 
    private void DetectPlayer()
    {
        //���� �÷��̾� ������Ʈ���� �Ÿ��� ��Ÿ����� ���ų� �۾����ٸ�
        if(Vector3.Distance(transform.position, target.transform.position) <= AttackRange)
        {
            //���� ���� ������ ���� ������
            isDetectedPlayer= true;

        }

    }



    //���� �Լ�
    private async Task Attack()
    {
        //animator.SetTrigger("Attack"); // ���� �ִϸ��̼� ����

        //�÷��̾�� ���ݷ¸�ŭ ������ �ο�
        Pmanager.playerHP -= status.EnemyATK;


        // �ִϸ��̼��� ���� ������ ���
        //float attackAnimTime = GetAnimationLength("�ִϸ��̼� �̸�");
        //await Task.Delay(Mathf.RoundToInt(attackAnimTime * 1000));


        //�ӽ� �ڵ�
        await Task.Delay(500);  //0.5�� ���
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

}
