using System.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject target;  // 이동할 목표 오브젝트
    public float moveSpeed = 5f;  // 이동 속도
    private bool isMoving = false; // 이동 여부 확인
    public bool isDetectedPlayer = false;   //플레이어 감지 여부
    private PlayerManger Pmanager;  //플레이어에게 데미지 처리를 위한 PlayerManager
    private EnemyStatus status; //이 적 케릭터의 스탯
    //private Animator animator; // 애니메이터(공격 에니메이션 추가 시 사용될 예정)

    [SerializeField] private float AttackRange = 0.1f;
    
    
    private void Start()
    {
        status = GetComponent<EnemyStatus>();
        // "Player" 태그가 있는 오브젝트를 찾아 타겟으로 설정
        target = GameObject.FindWithTag("Player");

        if(target != null)
        {
            //찾았으면 PlayerManger 받아오기
            Pmanager = target.GetComponent<PlayerManger>();

            //디버그용
            if(Pmanager != null)
            {
                Debug.Log("found player!");
            }
        }

        //플레이어 감지 상태를 false로 초기화
        isDetectedPlayer = false;

        //추후 공격 에니메이션 추가 시 사용될 용도
        //animator = GetComponent<Animator>();
    }

    public async Task Move()
    {
        if (target != null && !isMoving) // 타겟이 존재하고 이동 중이 아닐 때만 이동 시작
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
            }
        }
    }

    private void Update()
    {
        if (isMoving && target != null)
        {
            // 타겟 방향으로 이동
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

            // 목표 위치에 도달하면 이동 종료
            if (Vector3.Distance(transform.position, target.transform.position) < AttackRange)
            {
                isMoving = false;
            }
            //사거리 내에 플레이어가 있는지 확인
            DetectPlayer();
        }
    }

    public bool HasMoved()
    {
        return isMoving;
    }

    public void ResetMove()
    {
        isMoving = false; // 이동 상태 초기화
    }


    //플레이어가 사거리 내에 존재하는지 확인 
    private void DetectPlayer()
    {
        //만일 플레이어 오브젝트와의 거리가 사거리보다 같거나 작아진다면
        if(Vector3.Distance(transform.position, target.transform.position) <= AttackRange)
        {
            //감지 여부 변수의 값을 참으로
            isDetectedPlayer= true;

        }

    }



    //공격 함수
    private async Task Attack()
    {
        //animator.SetTrigger("Attack"); // 공격 애니메이션 실행

        //플레이어에게 공격력만큼 데미지 부여
        Pmanager.playerHP -= status.EnemyATK;


        // 애니메이션이 끝날 때까지 대기
        //float attackAnimTime = GetAnimationLength("애니메이션 이름");
        //await Task.Delay(Mathf.RoundToInt(attackAnimTime * 1000));


        //임시 코드
        await Task.Delay(500);  //0.5초 대기
    }


    //공격 함수에서 에니메이션 길이를 반환받기 위해 쓰이는 힘수
    //private float GetAnimationLength(string animationName)
    //{
    //    if (animator == null) return 1.0f; // 애니메이터 없을 경우 기본값

    //    //에니메이션을 이루는 클립들의 길이를 참조하기 위한 RuntimeAnimatorController
    //    RuntimeAnimatorController ac = animator.runtimeAnimatorController;

    //    foreach (AnimationClip clip in ac.animationClips)
    //    {
    //        //에니메이션을 이루는 각 클립별로
    //        if (clip.name == animationName)
    //            return clip.length;
    //    }
    //    return 1.0f; // 기본 애니메이션 길이 (1초)
    //}

}
