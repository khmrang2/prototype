using System.Collections;
using UnityEngine;

public class AfterSceneManager : MonoBehaviour
{
    public Door leftDoor, rightDoor;

    void Awake()
    {
        //## ������� Ű��
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