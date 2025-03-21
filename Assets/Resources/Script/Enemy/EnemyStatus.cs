using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    //적 스탯 목록, 보안 및 오류사항 감지를 위해 프포퍼티로 구성, inspector 창에서 확인 및 수정이 가능하도록 [SerializeField] 사용

    [SerializeField] private int enemyHP;   //적 체력
    [SerializeField] private int enemyATK;  //적 공격력
    [SerializeField] private int enemyDFF;  //적 방어력 (방어력 관련 내용은 노션의 팀스페이스 홈/ 기술 개발 문서 참조)


    //적 스탯 접근을 위한 프로퍼티

    public int EnemyHP {  get { return enemyHP; } set { enemyHP = value; } }
    public int EnemyATK { get { return enemyATK; } set { enemyATK = value; } }
    public int EnemyDFF { get { return enemyDFF; } set { enemyDFF = value; } }




    //혹시 몰라 만드는 적 생성 시에 쓸 수 있을 스탯 일괄적용 메소드
    public void SetEnemyStat(int HP, int ATK, int DFF) 
    {
        EnemyHP = HP;
        EnemyATK = ATK;
        EnemyDFF = DFF;
    }
}
