using System.Collections;
using UnityEngine;

public class AppControl : MonoBehaviour
{
    public static bool IsRestoreCompleted = false;

    public void QuitApp()
    {
        StartCoroutine(DelayAndExit());
    }

    IEnumerator DelayAndExit()
    {
        float timeout = 5f;
        float elapsed = 0f;

        while (!IsRestoreCompleted && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (IsRestoreCompleted)
        {
            Debug.Log("✅ 복구 완료 후 앱 종료");
        }
        else
        {
            Debug.LogError("⚠️ 복구 실패: 타임아웃");
        }

        Application.Quit();
    }

    void Start()
    {
        IsRestoreCompleted = false; // 초기화
    }
}