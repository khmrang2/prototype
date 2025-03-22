using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    public static Texture2D lastScreenTexture; // 캡처된 화면 이미지 저장
    public static string loadSceneName; // 다음 씬 이름 저장

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 현재 화면을 캡처하고 로딩 씬으로 이동
    /// </summary>
    public static void CaptureScreenAndLoad(string nextScene)
    {
        Instance.StartCoroutine(CaptureScreenCoroutine(nextScene));
    }

    /// <summary>
    /// `RenderTexture`를 사용하여 최적화된 캡처 수행 (안드로이드에서도 최적화됨)
    /// </summary>
    private static IEnumerator CaptureScreenCoroutine(string nextScene)
    {
        yield return new WaitForEndOfFrame(); // GPU 렌더링 완료 후 실행

        // 1. RenderTexture 생성
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        ScreenCapture.CaptureScreenshotIntoRenderTexture(rt);

        // 2. RenderTexture를 Texture2D로 변환
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        RenderTexture.active = rt;
        screenTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenTexture.Apply();
        RenderTexture.active = null; // RenderTexture 사용 해제

        // 3. 캡처한 이미지 저장
        lastScreenTexture = screenTexture;
        loadSceneName = nextScene;

        // 4. RenderTexture 메모리 해제 (안드로이드 최적화)
        rt.Release();
        Destroy(rt);

        // 5. 로딩 씬으로 비동기 이동
        SceneManager.LoadSceneAsync("LoadingScreen");
    }
    
}
