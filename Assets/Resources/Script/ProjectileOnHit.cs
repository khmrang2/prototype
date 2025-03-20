using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class ProjectileOnHit : MonoBehaviour
{

    private int atk;
    public int hitCount;

    private void Awake()
    {
        PlayerManger ps = FindObjectOfType<PlayerManger>();

        if (ps != null)
        {
            atk = ps.GetTotalDamage();
            Debug.Log("-----------Total Damage: " +  atk);
        }
        else { Debug.LogError("플레이어 스탯 참조 실패!"); }
    }

    //public string targetTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) 
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            enemy.status.EnemyHP -= atk;

            Destroy(this.gameObject);
        }

    }
    private void OnDestroy()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.NotifyProjectileDestroyed();
        }
    }
}
