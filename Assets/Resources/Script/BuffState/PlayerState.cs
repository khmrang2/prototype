using UnityEngine;
using System;

// 플레이어의 기본 상태를 관리하기 위한 플레이어 스테이트
public class PlayerState : BaseState
{

    [Header("Player Status Script")]
    public PlayerStatus playerStatus;

    [Header("버프 연동")]
    public int Pin_Hp;

    protected void Awake()
    {
        base.Awake();
        Player_Damage += playerStatus.PlayerATK;
        Player_Health += playerStatus.PlayerHP;
        Ball_Count += playerStatus.PlayerBallCnt;
        Pin_Hp += playerStatus.PlayerPinHP;
    }
}