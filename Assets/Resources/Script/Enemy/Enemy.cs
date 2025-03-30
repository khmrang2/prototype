using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy movement variables")]
    public float moveSpeed = 2f;  // 이동 속도
    [SerializeField] public bool isMoving = false; // 이동 여부 확인
    private int moveStep = 0; // 이동 단계 (5번으로 나누어 진행);
    private GameObject target;  // 추가된 변수, 타겟(플레이어)을 추적
    private float moveDistance; // 한 칸 이동 목표 거리
    public Transform start;
    public Vector3 spawn_offset;

    [Header("Enemy attack & hit variables")]
    public bool isDetectedPlayer = false;   //플레이어 감지 여부
    private PlayerManger Pmanager;  //플레이어에게 데미지 처리를 위한 PlayerManager

    public bool isSpawned = false;
    public bool isAlive;
    [SerializeField] private float AttackRange = 0.1f;

    private GameObject hpb;
    private RectTransform hpBarPos;
    private Slider hpBarSlider;

    [Header("Player Stat Script")]
    public PlayerState playerState;

    [Header("Enemy references")]
    public EnemyStatus status; //이 적 케릭터의 스탯
    public GameObject HpBar;    //체력바
    public GameObject canvas;   //체력바가 소환될 ui 캔버스

    [Header("Enemy Animation")]
    public Animator animator;
    public AnimationClip move_anim;
    public AnimationClip attack_anim;
    public AnimationClip hitted_anim;
    public AnimationClip death_anim;


    private void Start()
    {
        status = GetComponent<EnemyStatus>();
        // "Player" 태그가 있는 오브젝트를 찾아 타겟으로 설정
        target = GameObject.FindWithTag("Player");
        if (target != null)
        {
            //찾았으면 PlayerManager 받아오기
            Pmanager = target.GetComponent<PlayerManger>();

            //디버그용
            if (Pmanager != null)
            {
                Debug.Log("found player!");
            }
            else
            {
                Debug.Log("Player None");
            }
        }
        moveDistance = Vector3.Distance(start.position, target.transform.position) / 5f; // 한 칸 이동 목표 거리

        //플레이어 감지 상태를 false로 초기화
        isDetectedPlayer = false;
        //생존 처리를 true로
        isAlive = true;
        hpBarSlider.maxValue = status.EnemyHP;          //체력바 최대값 설정
        hpBarSlider.value = status.EnemyHP;
    }

    public async Task Move()
    {
        if (target != null && !isMoving) // 타겟이 존재하고 소환이 완료되었으며 이동 중이 아닐 때만 이동 시작
        {
            if (isSpawned)
            {
                //사거리 내에 플레이어가 존재하면
                if (isDetectedPlayer)
                {
                    await Attack(); //공격이 끝날 때까지 대기
                    isMoving = false;   //이동이 불필요하므로 false
                }
                else
                {
                    isMoving = true;    //사거리 내에 적이 들어와야 하니 이동
                    moveStep++;         // 이동 단계를 하나 증가
                    if (moveStep >= 5)  // 5턴마다 한 칸 이동
                    {
                        await MoveOneStep(); // 한 칸 이동
                        moveStep = 0; // 이동 단계를 초기화
                    }
                }
            }
            else
            {
                //스폰 처리가 안되었다면 이동하면 안되니 false
                isMoving = false;
            }
        }
    }

    private float duration = 0.2f; // 이동 시간
    public async Task MoveOneStep()
    {
        //타겟을 발견 했고, 필드에 소환되었으며 살아있는 경우에만 실행
        if (target == null || !isSpawned || !isAlive) return;

        isMoving = true; // 이동 시작
        //플레이어가 사거리 내에 없을 때만 실행
        if (!isDetectedPlayer && isAlive)
        {
            duration = move_anim.length;
            animator.SetTrigger("Enemy_Move");
            float elapsedTime = 0f;

            Vector3 startPosition = transform.position;
            Vector3 endPosition = transform.position - (Vector3.right * moveDistance);
            Debug.Log($"시작 {startPosition} 끝{endPosition}");
            Debug.Log($"이동거리{moveDistance}");
            while (elapsedTime < duration)
            {
                // 목표 위치로 일정 속도로 이동
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                await Task.Yield(); // 프레임마다 갱신
            }
            transform.position = endPosition; // 최종 위치 보정
            DetectPlayer(); // 이동 후 플레이어 감지
        }
        else
        {
            //플레이어가 사거리 내에 있다면
            await Attack(); //이동하지 않고 공격 수행, 공격이 끝날 때까지 대기
        }
        Debug.Log("isMoving = false");
        isMoving = false;   //이동이 불필요하므로 false

    }

    private void Update()
    {
        //체력바 위치 업데이트
        LocateEnemyHealthBar();
        //hpBarPos.localScale = Vector3.one * (Screen.height / 2340.0f);    //화면 비율에 따라 체력바 크기 조정 확인용 디버그 코드
        /*
        //살아 있을때만 실행
        if (isAlive & status != null)
        {
            if (status.EnemyHP < 0) //만약 체력이 0 이하로 떨어지면 사망처리
            {
                isAlive = false;
                //OnDie();를 enemy_death 애니메이션의 끝 트리거에 넣음.
                animator.SetTrigger("Enemy_Death");
                if (playerState.Player_Generation != 0)
                {
                    Pmanager.playerHP = (int)Mathf.Min(Pmanager.playerHP + playerState.Player_Generation * 10, Pmanager.maxHP);
                    Debug.Log($"현재 체력{Pmanager.playerHP}");
                }

            }
        }
        */
        if (!isMoving) return; // 이동 중이 아닐 때만 실행

        // 사거리 내에 플레이어가 있는지 확인
        DetectPlayer();
    }

    public bool HasMoved()
    {
        return !isMoving;  // 이동이 끝났다면 false 반환
    }

    public void ResetMove()
    {
        isMoving = false; // 이동 상태 초기화
    }

    //플레이어가 사거리 내에 존재하는지 확인
    private void DetectPlayer()
    {
        if (Vector3.Distance(transform.position, target.transform.position) <= AttackRange)
        {
            isDetectedPlayer = true;
        }
    }

    //공격 함수
    private async Task Attack()
    {
        if (!isAlive) return;
        animator.SetTrigger("Enemy_Attack");
        await Task.Delay((int)(attack_anim.length * 800));
        Pmanager.getHitted(status.EnemyATK);
        // 임시 코드 (공격 애니메이션을 위한 대기 시간)
        // 플레이어에게 공격력만큼 데미지 부여
    }

    // 데미지 함수.
    public async Task getDamage()
    {
        animator.SetTrigger("Enemy_Hitted");
        //체력바 값 업데이트
        hpBarSlider.value = status.EnemyHP;

        //살아 있을때만 실행
        if (isAlive & status != null)
        {
            Debug.LogWarning("살아 있는가?");
            if (status.EnemyHP < 0) //만약 체력이 0 이하로 떨어지면 사망처리
            {
                isAlive = false;
                if (playerState.Player_Generation != 0)
                {
                    Pmanager.playerHP = (int)Mathf.Min(Pmanager.playerHP + playerState.Player_Generation * 10, Pmanager.maxHP);
                    Debug.Log($"현재 체력{Pmanager.playerHP}");
                }
                await OnDie();
            }
            else
            {
                await Task.Delay((int)(hitted_anim.length * 1000));
            }
        }
    }


    //체력바 소환 함수
    public void SetEnemyHealthBar()
    {
        if (canvas != null)
        {
            hpb = Instantiate(HpBar, canvas.transform);     //체력바 소환
            hpb.SetActive(true);
            hpBarPos = hpb.GetComponent<RectTransform>();   //체력바 위치 이동을 위해 RectTransform을 받아옴
            hpBarSlider = hpb.GetComponent<Slider>();       //체력바 값 설정을 위해 slider 컴포넌트를 받아옴
            float minWidth = 480f;
            float maxWidth = 1440f;
            float t = Mathf.InverseLerp(minWidth, maxWidth, Screen.width);
            float scaleValue = Mathf.Lerp(0.4f, 0.8f, t);
            hpBarPos.localScale = Vector3.one * scaleValue;  //화면 비율에 맞춰 체력바의 크기 조정
        }
        else
        {
            Debug.LogError("Can't find canvas for enemy hpbar!");
        }
    }



    //체력바 위치를 적 케릭터 상단으로 조정하는 함수
    private void LocateEnemyHealthBar()
    {
        if (canvas != null)
        {
            Collider2D collider = GetComponent<Collider2D>();
            float heightOffset = collider.bounds.size.y; // collider의 높이를 기준으로 offset 설정

            Vector3 worldPos = transform.position + Vector3.up * heightOffset;
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(worldPos);

            hpBarPos.anchorMin = viewportPos;
            hpBarPos.anchorMax = viewportPos;

            //Vector3 viewportPos = Camera.main.WorldToViewportPoint(this.gameObject.transform.position + Vector3.up * 0.9f);   //체력바가 위치할 좌표
            //hpBarPos.anchorMin = viewportPos;
            //hpBarPos.anchorMax = viewportPos;
        }
    }


    //사망 시 작동하는 함수
    private async Task OnDie()
    {
        ResetAllTriggers();
        animator.SetTrigger("Enemy_Death");
        await Task.Delay((int)(death_anim.length * 1000));

        hpb.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void Get_Pin_Damage(int Damage)
    {
        if (isSpawned) status.EnemyHP -= Damage;
        Debug.Log("데미지 들어갑니다");
    }
    private void ResetAllTriggers()
    {
        animator.ResetTrigger("Enemy_Hitted");
        animator.ResetTrigger("Enemy_Death");
        animator.ResetTrigger("Move");
        animator.ResetTrigger("Attack");
    }
}


/*
     }

    // 공격 함수에서 에니메이션 길이를 반환받기 위해 쓰이는 힘수
    // 콜링된 애니메이션의 길이를 받아서 어디까지 써라. 
    private float GetAnimationLength(string animationName)
    {
        if (animator == null) return 1.0f; // 애니메이터 없을 경우 기본값

        //에니메이션을 이루는 클립들의 길이를 참조하기 위한 RuntimeAnimatorController
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;

        foreach (AnimationClip clip in ac.animationClips)
        {
            //에니메이션을 이루는 각 클립별로
            if (clip.name == animationName)
                return clip.length;
        }
        return 1.0f; // 기본 애니메이션 길이 (1초)
    }

 
 */