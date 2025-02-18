using System.Collections;
using UnityEngine;

public class AfterSceneManager : MonoBehaviour
{
    public Door leftDoor, rightDoor;

    void Awake()
    {
        StartCoroutine(OpenDoors());
    }

    private IEnumerator OpenDoors()
    {
        yield return new WaitForSeconds(0.02f);
        Debug.LogError("씬 전환 성공. 문 열기 시작.");
        leftDoor.onclicked();
        rightDoor.onclicked();

        while (!leftDoor.isFinished() || !rightDoor.isFinished())
        {
            yield return null;
        }

        Debug.LogError("문이 완전히 열렸습니다.");
        leftDoor.setFalse();
        rightDoor.setFalse();
    }
}