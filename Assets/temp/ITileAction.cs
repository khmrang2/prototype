using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileAction
{
    bool OnPlayerEnter(PlayerControl player);

    bool OnEnemyEnter(EnemyMovement enemy);
}
