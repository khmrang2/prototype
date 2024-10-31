using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTileAction : MonoBehaviour, ITileAction
{
    [SerializeField] public GameObject[] enemyList = null;
    [SerializeField] private GameObject menu = null;

    public bool OnPlayerEnter(PlayerControl player)
    {
        Debug.Log(" Goal tile has reached!");
        if (ClearCheck())
        {
            //클리어 시 작동할 함수 작성
            Time.timeScale = 0;
            Debug.Log("Game Clear!");
            menu.SetActive(true);
        }
        return true;
    }

    public bool OnEnemyEnter(EnemyMovement enemy)
    {
        return true;
    }


    public bool ClearCheck() { 

        for(int i = 0; i < enemyList.Length; i++)
        {
            EnemyMovement em = enemyList[i].GetComponent<EnemyMovement>();
            if (em.isDead == false)
            {
                return false;
            }
        }
        return true;
    }


}

