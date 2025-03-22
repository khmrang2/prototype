using System.Collections;
using UnityEngine;

public class AfterSceneManager : MonoBehaviour
{
    public Door leftDoor, rightDoor;

    void Awake()
    {
        //## 출시전에 키기
        //leftDoor.setActive();
        //rightDoor.setActive();

        StartCoroutine(OpenDoors());
    }

    private IEnumerator OpenDoors()
    {
        yield return new WaitForSeconds(0.02f);
        leftDoor.onclicked();
        rightDoor.onclicked();

        while (!leftDoor.isFinished() || !rightDoor.isFinished())
        {
            yield return null;
        }

        leftDoor.setFalse();
        rightDoor.setFalse();
    }
}