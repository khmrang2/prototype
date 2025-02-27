using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    //�� ���� ���, ���� �� �������� ������ ���� ������Ƽ�� ����, inspector â���� Ȯ�� �� ������ �����ϵ��� [SerializeField] ���

    [SerializeField] private int enemyHP;   //�� ü��
    [SerializeField] private int enemyATK;  //�� ���ݷ�
    [SerializeField] private int enemyDFF;  //�� ���� (���� ���� ������ ����� �������̽� Ȩ/ ��� ���� ���� ����)


    //�� ���� ������ ���� ������Ƽ

    public int EnemyHP {  get { return enemyHP; } set { enemyHP = value; } }
    public int EnemyATK { get { return enemyATK; } set { enemyATK = value; } }
    public int EnemyDFF { get { return enemyDFF; } set { enemyDFF = value; } }




    //Ȥ�� ���� ����� �� ���� �ÿ� �� �� ���� ���� �ϰ����� �޼ҵ�
    public void SetEnemyStat(int HP, int ATK, int DFF) 
    {
        EnemyHP = HP;
        EnemyATK = ATK;
        EnemyDFF = DFF;
    }

}
