using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public Vector3 moveDir = Vector3.right;
    public float moveSpeed = 0.5f;
    public Vector3 posLimitMin = Vector3.zero;
    public Vector3 posLimitMax = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = this.gameObject.transform.position + moveDir * moveSpeed * Time.deltaTime;

        if (isPassedLimit())
        {
            Destroy(this.gameObject);
        }
    }


    bool isPassedLimit()
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
