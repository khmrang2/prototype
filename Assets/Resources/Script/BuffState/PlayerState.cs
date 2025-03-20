using GooglePlayGames.BasicApi;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

// 플레이어의 기본 상태를 관리하기 위한 플레이어 스테이트
public class PlayerState : BaseState
{
    // 저는 PLayerState를 Ball.cs 나 Pin.cs에 가서 public으로 할당해준 다음
    // 스텟을 참조하여 값을 수정하고 업데이트하는 방식을 선택햇씁니다.

    //플레이어 스탯 참조용 스크립트, (Unity 에디터에서 할당)
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