using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    public static Texture2D lastScreenTexture; // ĸó�� ȭ�� �̹��� ����
    public static string loadSceneName; // ���� �� �̸� ����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� �ٲ� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ���� ȭ���� ĸó�ϰ� �ε� ������ �̵�
    /// </summary>
    public static void CaptureScreenAndLoad(string nextScene)
    {
        Instance.StartCoroutine(CaptureScreenCoroutine(nextScene));
    }

    /// <summary>
    /// `RenderTexture`�� ����Ͽ� ����ȭ�� ĸó ���� (�ȵ���̵忡���� ����ȭ��)
    /// </summary>
    private static IEnumerator CaptureScreenCoroutine(string nextScene)
    {
        yield return new WaitForEndOfFrame(); // GPU ������ �Ϸ� �� ����

        // 1. RenderTexture ����
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        ScreenCapture.CaptureScreenshotIntoRenderTexture(rt);

        // 2. RenderTexture�� Texture2D�� ��ȯ
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        RenderTexture.active = rt;
        screenTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenTexture.Apply();
        RenderTexture.active = null; // RenderTexture ��� ����

        // 3. ĸó�� �̹��� ����
        lastScreenTexture = screenTexture;
        loadSceneName = nextScene;

        // 4. RenderTexture �޸� ���� (�ȵ���̵� ����ȭ)
        rt.Release();
        Destroy(rt);

        // 5. �ε� ������ �񵿱� �̵�
        SceneManager.LoadSceneAsync("LoadingScreen");
    }
}
