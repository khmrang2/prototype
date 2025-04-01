using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentHandController : MonoBehaviour
{
    public Transform needleTransform;
    public List<float> rotationAngles;

    private Coroutine revertCoroutine;

    /// <summary>
    /// 잠깐 보여주고 다시 원래 위치로 돌아오는 연출
    /// </summary>
    public void ShowTempStage(int selectedIndex, int unlockedIndex, float revertDelay = 1.2f)
    {
        // 일단 선택된 위치로 바늘 움직임
        SetStage(selectedIndex);

        // 기존 되돌리는 코루틴이 있다면 멈춤
        if (revertCoroutine != null)
        {
            StopCoroutine(revertCoroutine);
        }

        // 되돌리는 코루틴 시작
        revertCoroutine = StartCoroutine(RevertNeedle(unlockedIndex, revertDelay));
    }

    private IEnumerator RevertNeedle(int targetIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SetStage(targetIndex);
    }

    public void SetStage(int stageIndex)
    {
        if (stageIndex >= 0 && stageIndex < rotationAngles.Count)
        {
            float zAngle = rotationAngles[stageIndex];
            needleTransform.localEulerAngles = new Vector3(0f, 0f, zAngle);
        }
    }
}
