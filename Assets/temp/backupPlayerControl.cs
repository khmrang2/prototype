using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backupPlayerControl : MonoBehaviour
{
    public bool movable = false;
    public float moveDistance = 0.5f;
    [SerializeField] private GameObject Enemy = null;
    EnemyMovement enemyCtr = null;


    private void Start()
    {
        enemyCtr = Enemy.GetComponent<EnemyMovement>();
    }



    public void MoveUp()
    {
        if (movable)
        {
            Vector3 pos = this.transform.position;
            pos.y += moveDistance;
            this.transform.position = pos;
            movable = false;
        }
        enemyCtr.EnemyMove();
    }


    public void MoveDown()
    {
        if (movable)
        {
            Vector3 pos = this.transform.position;
            pos.y -= moveDistance;
            this.transform.position = pos;
            movable = false;
        }
        enemyCtr.EnemyMove();
    }

    public void MoveRight()
    {
        if (movable)
        {
            Vector3 pos = this.transform.position;
            pos.x += moveDistance;
            this.transform.position = pos;
            movable = false;
        }
        enemyCtr.EnemyMove();
    }

    public void MoveLeft()
    {
        if (movable)
        {
            Vector3 pos = this.transform.position;
            pos.x -= moveDistance;
            this.transform.position = pos;
            movable = false;
        }
        enemyCtr.EnemyMove();
    }

}
