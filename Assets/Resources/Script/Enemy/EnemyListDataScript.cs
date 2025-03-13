using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataList", menuName = "ScriptableObject/EnemyDataList", order = 1)]

public class EnemyDataList : ScriptableObject 
{
    //적들 생성 시 사용될 적들의 데이터 리스트를 담을 스크립터블 오브젝트

    //각 리스트는 순서대로 해당 맵에서 등장할 적들의 체력, 공격력, 방어력을 담은 리스트

    //각 리스트의 인덱스 번호가 곧 적의 번호 (예: 첫번째 적의 스탯은 각 리스트의 0번 값)


    public List<int> EnemyHPList;

    public List<int> EnemyATKList;

    public List<int> EnemyDFFList;

    //이번 스테이지에 등장할 적들의 리스트, 앞에서 부터 아래 EnemySpawnCountPerTurn의 값에 따라 등장
    public List<GameObject> EnemyList;

    //각 턴마다 소환될 적의 수, 인덱스 번호가 턴, 해당 인덱스 번호의 값이 해당 턴에 소환될 적의 수
    public List<int> EnemySpawnCountPerTurn;
}

