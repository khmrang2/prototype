 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 3.0f;
    private Vector3 cameraPos = Vector3.zero;
    private Vector3 targetPos = Vector3.zero;

    private void Start()
    {
        cameraPos = transform.position;
        targetPos = target.transform.position;
  
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(cameraPos.x - targetPos.x + target.transform.position.x,
            cameraPos.y - targetPos.y + target.transform.position.y, -10);
            
                                                                                                              
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
        
    }
}
