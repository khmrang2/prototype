using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;

public class ProjectileOnHit : MonoBehaviour
{
    [Header("Projrctile Variables")]
    public bool isDestroyed;
    public Transform returnTransform;
    public Transform InstantiateTransform;

    [Header("Projectile Movement Variables")]
    public Vector3 moveDir = Vector3.right;
    public float moveSpeed = 0.5f;
    public Vector3 posLimitMin = Vector3.zero;
    public Vector3 posLimitMax = Vector3.zero;


    [Header("Scripts")]
    public PlayerManger playerManager;
    public PlayerState pstate;
    public GameManager gameManager;



    private void Start()
    {
        //���� �ʱ�ȭ
        isDestroyed = false;
    }


    private void Update()
    {
        if (!isDestroyed) 
        {
            //����ִ� ������ ���� �۵�
            this.gameObject.transform.position = this.gameObject.transform.position + moveDir * moveSpeed * Time.deltaTime; //�̵�

            if (isPassedLimit())
            {
                //���� ȭ�� ������ �����ٸ�
                RegarndAsDestroyed();   //�ı�ó��
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) 
        {
            //���� �浹 �� �۵�

            if (!isDestroyed)
            {
                //�ı����� �ʾ��� ���� ������ ó��

                Enemy enemy = collision.gameObject.GetComponent<Enemy>();   //�浹�� ���� ������ ��������

                int damage = CalculteDamage();      //������ ���
                enemy.status.EnemyHP -= damage;     //���� ������ ��ŭ ���� �� ü�� ����

                if (!pstate.Ball_Pierce_Power)
                {
                    //������ Ȱ��ȭ ���°� �ƴ϶�� �� ���� �� �ı�ó��
                    RegarndAsDestroyed();
                }
            }
        }

    }


    //���� ���� �Լ�
    public void StartAttack()
    {
        this.gameObject.transform.position = InstantiateTransform.position; //������ ���󰡱� �����ϴ� ��ġ�� �̵�
        isDestroyed = false;    //�����̰� �ϱ� ���� �ı� ������ ��������
    }



    //������ �� �ޱ�
    private int CalculteDamage()
    {
        if (playerManager != null)
        {
            int atk = playerManager.GetTotalDamage();
            Debug.Log("-----------Total Damage: " + atk);
            return atk;
        }
        else 
        { 
            Debug.LogError("�÷��̾� ���� ���� ����!"); 
            return 0;
        }
    }

    //�ı�ó��
    private void RegarndAsDestroyed()
    {
        isDestroyed = true; //�������� �ʵ��� �ı� ������ ������
        this.gameObject.transform.position = returnTransform.position;  //�ʱ� ��ġ�� �̵�
        gameManager.NotifyProjectileDestroyed();    //������ ���� ���� �ý��ۿ� �ı��Ǿ��ٰ� �˸�
    }


    //ȭ�� ������ �������� ó��
    private bool isPassedLimit()
    {
        Vector3 temp = this.transform.position;

        if (temp.x >= posLimitMax.x ||
            temp.x <= posLimitMin.x ||
            temp.y >= posLimitMax.y ||
            temp.y <= posLimitMin.y)
        {
            return true;
        }
        else { return false; }
    }

}
