using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTileAction : MonoBehaviour, ITileAction
{
    public bool OnPlayerEnter(PlayerControl player)
    {
        Debug.Log(" Movement blocked by obstacle");
        return false;
    }

    public bool OnEnemyEnter(EnemyMovement enemy)
    {
        return false;
    }
}

