using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTileAction : MonoBehaviour, ITileAction
{
    public bool OnPlayerEnter(PlayerControl player)
    {
        Debug.Log("you contacted with Enemy");
        return true;
    }

    public bool OnEnemyEnter(EnemyMovement enemy)
    {
        return true;
    }
}
