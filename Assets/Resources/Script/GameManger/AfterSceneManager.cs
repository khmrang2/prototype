using System.Collections;
using UnityEngine;

public class AfterSceneManager : MonoBehaviour
{
    public Door leftDoor, rightDoor;

    void Awake()
    {
<<<<<<< Updated upstream
        //## 출시전에 키기
=======
        //## ������� Ű��
>>>>>>> Stashed changes
        //leftDoor.setActive();
        //rightDoor.setActive();

        StartCoroutine(OpenDoors());
    }

    private IEnumerator OpenDoors()
    {
        yield return new WaitForSeconds(0.02f);
        leftDoor.onclicked();
        rightDoor.onclicked();
        
        yield return new WaitForSeconds(1f);

        while (!leftDoor.isFinished() || !rightDoor.isFinished())
        {
            yield return null;
        }

        leftDoor.setFalse();
        rightDoor.setFalse();
    }
}