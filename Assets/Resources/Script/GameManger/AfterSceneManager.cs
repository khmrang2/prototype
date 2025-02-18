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
        Debug.LogError("�� ��ȯ ����. �� ���� ����.");
        leftDoor.onclicked();
        rightDoor.onclicked();

        while (!leftDoor.isFinished() || !rightDoor.isFinished())
        {
            yield return null;
        }

        Debug.LogError("���� ������ ���Ƚ��ϴ�.");
        leftDoor.setFalse();
        rightDoor.setFalse();
    }
}