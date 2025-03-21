using GooglePlayGames.BasicApi;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

// �÷��̾��� �⺻ ���¸� �����ϱ� ���� �÷��̾� ������Ʈ
public class PlayerState : BaseState
{
    // ���� PLayerState�� Ball.cs �� Pin.cs�� ���� public���� �Ҵ����� ����
    // ������ �����Ͽ� ���� �����ϰ� ������Ʈ�ϴ� ����� �����޾��ϴ�.

    //�÷��̾� ���� ������ ��ũ��Ʈ, (Unity �����Ϳ��� �Ҵ�)
    [Header("Player Status Script")]
    public PlayerStatus playerStatus;

    [Header("���� ����")]

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