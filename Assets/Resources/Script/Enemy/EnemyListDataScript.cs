using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataList", menuName = "ScriptableObject/EnemyDataList", order = 1)]

public class EnemyDataList : ScriptableObject 
{
    //���� ���� �� ���� ������ ������ ����Ʈ�� ���� ��ũ���ͺ� ������Ʈ

    //�� ����Ʈ�� ������� �ش� �ʿ��� ������ ������ ü��, ���ݷ�, ������ ���� ����Ʈ

    //�̹� ���������� ������ ������ ����Ʈ, �տ��� ���� �Ʒ� EnemySpawnCountPerTurn�� ���� ���� ����
    public List<EnemyData> EnemyList;

    //�� �ϸ��� ��ȯ�� ���� ��, �ε��� ��ȣ�� ��, �ش� �ε��� ��ȣ�� ���� �ش� �Ͽ� ��ȯ�� ���� ��
    public List<int> EnemySpawnCountPerTurn;
}
[System.Serializable]
public struct EnemyData
{
    public int hp;
    public int attack;
    public int defense;
    public GameObject enemyType;
}


