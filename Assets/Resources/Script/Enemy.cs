using System.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;  // 이동 속도
    [SerializeField] public bool isMoving = false; // 이동 여부 확인
    public bool isDetectedPlayer = false;   //플레이어 감지 여부
    private PlayerManger Pmanager;  //플레이어에게 데미지 처리를 위한 PlayerManager
    private EnemyStatus status; //이 적 케릭터의 스탯
    public bool isSpawned = false;
    public bool isAlive;
    [SerializeField] private float AttackRange = 0.1f;
    private Vector3 targetPosition;  // 목표 위치
    private int moveStep = 0; // 이동 단계 (5번으로 나누어 진행);
    private GameObject target;  // 추가된 변수, 타겟(플레이어)을 추적
    float moveDistance; // 한 칸 이동 목표 거리
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
        }
        moveDistance = Vector3.Distance(transform.position, target.transform.position) / 5f; // 한 칸 이동 목표 거리
        //플레이어 감지 상태를 false로 초기화
        isDetectedPlayer = false;
        //생존 처리를 true로
        isAlive = true;
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
        if (target == null || !isSpawned) return;
        isMoving = true; // 이동 시작
        float elapsedTime = 0f;
        
        
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position - (Vector3.right * moveDistance);

        while (elapsedTime < duration)
        {
            // 목표 위치로 일정 속도로 이동
            Debug.Log($"{Time.deltaTime} 시간");
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            await Task.Yield(); // 프레임마다 갱신
        }
        transform.position = endPosition; // 최종 위치 보정
        isMoving = false; // 이동 종료

        DetectPlayer(); // 이동 후 플레이어 감지
    }

    private void Update()
    {
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
        // 플레이어에게 공격력만큼 데미지 부여
        Pmanager.playerHP -= status.EnemyATK;

        // 임시 코드 (공격 애니메이션을 위한 대기 시간)
        await Task.Delay(500);  // 0.5초 대기
    }

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