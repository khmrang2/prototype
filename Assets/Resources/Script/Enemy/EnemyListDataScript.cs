using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataList", menuName = "ScriptableObject/EnemyDataList", order = 1)]

public class EnemyDataList : ScriptableObject 
{
    //���� ���� �� ���� ������ ������ ����Ʈ�� ���� ��ũ���ͺ� ������Ʈ

    //�� ����Ʈ�� ������� �ش� �ʿ��� ������ ������ ü��, ���ݷ�, ������ ���� ����Ʈ

    //�� ����Ʈ�� �ε��� ��ȣ�� �� ���� ��ȣ (��: ù��° ���� ������ �� ����Ʈ�� 0�� ��)


    public List<int> EnemyHPList;

    public List<int> EnemyATKList;

    public List<int> EnemyDFFList;

    //�̹� ���������� ������ ������ ����Ʈ, �տ��� ���� �Ʒ� EnemySpawnCountPerTurn�� ���� ���� ����
    public List<GameObject> EnemyList;

    //�� �ϸ��� ��ȯ�� ���� ��, �ε��� ��ȣ�� ��, �ش� �ε��� ��ȣ�� ���� �ش� �Ͽ� ��ȯ�� ���� ��
    public List<int> EnemySpawnCountPerTurn;
}

