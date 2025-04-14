using System.Collections;
using UnityEngine;

public class AppControl : MonoBehaviour
{
    public static bool IsRestoreCompleted = false;

    [SerializeField]
    private DataControl datactr;

    public void QuitApp()
    {
        datactr.SaveDataWithCallback(success => {
            if (success)
            {
                Application.Quit();
            }
            else
            {
                Debug.Log("저장 실패!");
            }
        });
    }

    public void forceQuitApp()
    {
        Application.Quit();
    }


    void Start()
    {
        IsRestoreCompleted = false; // 초기화
    }
}