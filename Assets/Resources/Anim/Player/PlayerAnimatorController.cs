using UnityEngine;

public class PlayerAnimatorMobile : MonoBehaviour
{
    public Animator animator;
    public float idleTimer = 0f;
    public float idleThreshold = 3f;
    private bool isInSpecialIdle = false;

    void Update()
    {
        // 터치 입력 감지
        bool hasTouch = Input.touchCount > 0;
        

        if (!hasTouch)
        {
            Debug.Log("터치 감지됨!1");
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleThreshold && !isInSpecialIdle)
            {
                int randomIndex = Random.Range(1, 4); // 1~3
                animator.SetInteger("SpecialIdleIdx", randomIndex);
                isInSpecialIdle = true; // Special Idle 진입했음
                idleTimer = 0f;
            }
        }
        else
        {
            // 터치가 들어오면 바로 IdleStill로 돌아가기
            animator.SetInteger("SpecialIdleIdx", 0); // 0이면 아무 것도 안 함
            isInSpecialIdle = false;
            idleTimer = 0f;
        }
    }

    // 애니메이션 이벤트에서 호출: Special Idle 끝나면 Idle로 복귀
    public void ReturnToIdle()
    {
        animator.SetInteger("SpecialIdleIdx", 0);
        isInSpecialIdle = false;
    }
    public void TriggerAttack()
    {
        animator.SetTrigger("Trigger_Attack");
    }

    public void TriggerDamage()
    {
        animator.SetTrigger("Trigger_Damage");
    }
}