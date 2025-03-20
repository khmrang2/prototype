using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy movement variables")]
    public float moveSpeed = 2f;  // �̵� �ӵ�
    [SerializeField] public bool isMoving = false; // �̵� ���� Ȯ��
    private int moveStep = 0; // �̵� �ܰ� (5������ ������ ����);
    private GameObject target;  // �߰��� ����, Ÿ��(�÷��̾�)�� ����
    private float moveDistance; // �� ĭ �̵� ��ǥ �Ÿ�
    public Transform start;

    [Header("Enemy attack & hit variables")]
    public bool isDetectedPlayer = false;   //�÷��̾� ���� ����
    private PlayerManger Pmanager;  //�÷��̾�� ������ ó���� ���� PlayerManager
    
    public bool isSpawned = false;
    public bool isAlive;
    [SerializeField] private float AttackRange = 0.1f;

    public GameObject hpb;
    private RectTransform hpBarPos;
    private Slider hpBarSlider;


    [Header("Enemy references")]
    public EnemyStatus status; //�� �� �ɸ����� ����
    public GameObject HpBar;    //ü�¹�
    public GameObject canvas;   //ü�¹ٰ� ��ȯ�� ui ĵ����



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
            else
            {
                Debug.Log("Player None");
            }
        }
        moveDistance = Vector3.Distance(start.position, target.transform.position) / 5f; // �� ĭ �̵� ��ǥ �Ÿ�

        //�÷��̾� ���� ���¸� false�� �ʱ�ȭ
        isDetectedPlayer = false;
        //���� ó���� true��
        isAlive = true;
        hpBarSlider.maxValue = status.EnemyHP;          //ü�¹� �ִ밪 ����


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
        //Ÿ���� �߰� �߰�, �ʵ忡 ��ȯ�Ǿ����� ����ִ� ��쿡�� ����
        if (target == null || !isSpawned || !isAlive) return;

        //�÷��̾ ��Ÿ� ���� ���� ���� ����
        if (!isDetectedPlayer)
        {
            isMoving = true; // �̵� ����
            float elapsedTime = 0f;

            Vector3 startPosition = transform.position;
            Vector3 endPosition = transform.position - (Vector3.right * moveDistance);
            Debug.Log($"���� {startPosition} ��{endPosition}");
            Debug.Log($"�̵��Ÿ�{moveDistance}");
            while (elapsedTime < duration)
            {
                // ��ǥ ��ġ�� ���� �ӵ��� �̵�
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                await Task.Yield(); // �����Ӹ��� ����
            }
            transform.position = endPosition; // ���� ��ġ ����
            isMoving = false; // �̵� ����

            DetectPlayer(); // �̵� �� �÷��̾� ����
        }
        else
        {
            //�÷��̾ ��Ÿ� ���� �ִٸ�

            await Attack(); //�̵����� �ʰ� ���� ����, ������ ���� ������ ���
            isMoving = false;   //�̵��� ���ʿ��ϹǷ� false
        }

    }

    private void Update()
    {
        //ü�¹� ��ġ ������Ʈ
        LocateEnemyHealthBar();
        //hpBarPos.localScale = Vector3.one * (Screen.height / 2340.0f);    //ȭ�� ������ ���� ü�¹� ũ�� ���� Ȯ�ο� ����� �ڵ�
        
        //ü�¹� �� ������Ʈ
        hpBarSlider.value = status.EnemyHP;

        //��� �������� ����
        if (isAlive & status != null)
        {
            if(status.EnemyHP < 0) 
            {
                //���� ü���� 0 ���Ϸ� �������� ���ó��
                isAlive = false;
                OnDie();
            }
        }

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



    //ü�¹� ��ȯ �Լ�
    public void SetEnemyHealthBar() 
    {
        if (canvas != null)
        {
            hpb = Instantiate(HpBar, canvas.transform);     //ü�¹� ��ȯ
            hpBarPos = hpb.GetComponent<RectTransform>();   //ü�¹� ��ġ �̵��� ���� RectTransform�� �޾ƿ�
            hpBarSlider = hpb.GetComponent<Slider>();       //ü�¹� �� ������ ���� slider ������Ʈ�� �޾ƿ�
            hpBarPos.localScale = Vector3.one * (Screen.height / 2340.0f);  //ȭ�� ������ ���� ü�¹��� ũ�� ����
        }
        else 
        {
            Debug.LogError("Can't find canvas for enemy hpbar!");
        }
    }



    //ü�¹� ��ġ�� �� �ɸ��� ������� �����ϴ� �Լ�
    private void LocateEnemyHealthBar()
    {
        if (canvas != null) 
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(this.gameObject.transform.position + Vector3.up * 0.9f);   //ü�¹ٰ� ��ġ�� ��ǥ
            hpBarPos.anchorMin = viewportPos;
            hpBarPos.anchorMax = viewportPos;
        }
    }


    //��� �� �۵��ϴ� �Լ�
    public void OnDie()
    {
        HpBar.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
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