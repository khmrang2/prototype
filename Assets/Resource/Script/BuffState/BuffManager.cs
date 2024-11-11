using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public GameManager gameManager; // 게임 매니저 참조


    PlayerState playerDefaultState = null;

    [SerializeField] private BuffState buffState; // buffState 참조
    private List<BuffEffect> allBuffs = new List<BuffEffect>(); // 모든 활성화된 버프
    private List<BuffEffect> newBuffs = new List<BuffEffect>(); // 새로 추가된 버프

    private void Start()
    {
        // 플레이어 스테이트를 최초 생성 해서 가져옴.
        // => json으로 load해서 플레이어의 설정을 가져오는 형식으로 바꿀 수 있음.
        playerDefaultState = new PlayerState();
    }

    // 새 버프를 추가하고 newBuffs에 저장
    public void addBuff(BuffEffect buff)
    {
        newBuffs.Add(buff);
    }

    // 새 버프에 대해서 업데이트 해주어 buffState를 업데이트.
    public BuffState updateBuffState()
    {
        foreach (BuffEffect buff in newBuffs)
        {
            ApplyBuffEffectToPlayer(buff);
            allBuffs.Add(buff); // 적용한 버프를 allBuffs로 이동
        }

        newBuffs.Clear(); // newBuffs 초기화
        return buffState;
    }

    // 인자로 들어온 버프를 계산하는 메소드
    private void ApplyBuffEffectToPlayer(BuffEffect buff)
    {
        if (buff.Player_Damage.HasValue)
            buffState.Player_Damage += buff.Player_Damage.Value;

        if (buff.Player_CriticalChance.HasValue)
            buffState.Player_CriticalChance += buff.Player_CriticalChance.Value;

        if (buff.Player_HealthIncrease.HasValue)
            buffState.Player_HealthIncrease += buff.Player_HealthIncrease.Value;

        if (buff.Player_DoubleUpChance.HasValue)
            buffState.Player_DoubleUpChance += buff.Player_DoubleUpChance.Value;

        if (buff.Player_ShieldPower.HasValue)
            buffState.Player_ShieldPower += buff.Player_ShieldPower.Value;

        if (buff.Ball_Size.HasValue)
            buffState.Ball_Size += buff.Ball_Size.Value;

        if (buff.Ball_Count.HasValue)
            buffState.Ball_Count += buff.Ball_Count.Value;

        if (buff.Ball_Elasticity.HasValue)
            buffState.Ball_Elasticity += buff.Ball_Elasticity.Value;

        if (buff.Ball_PiercePower.HasValue)
            buffState.Ball_PiercePower += buff.Ball_PiercePower.Value;

        if (buff.Ball_BallSplitCount.HasValue)
            buffState.Ball_BallSplitCount += buff.Ball_BallSplitCount.Value;
    }

    public BuffState getBuffState()
    {
        return this.buffState;
    }
}