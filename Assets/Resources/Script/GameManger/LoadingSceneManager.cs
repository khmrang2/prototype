using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEditor.PackageManager;

public class LoadingSceneManager : MonoBehaviour
{
    public Door leftDoor, rightDoor;
    public RawImage capturedImage; // 이전 씬에서 불러온 첫 프레임.
    public float transitionDuration = 1.5f;

    private AsyncOperation asyncLoad;
    private RenderTexture sceneRenderTexture; // 씬 첫 프레임을 저장할 RenderTexture

    void Start()
    {
        if (SceneTransitionManager.lastScreenTexture != null)
        {
            capturedImage.texture = SceneTransitionManager.lastScreenTexture;
            capturedImage.gameObject.SetActive(true);
            capturedImage.rectTransform.localScale = new Vector3(1, -1, 1); // 이미지 반전 해결
        }
        StartCoroutine(CloseDoorsAndLoadNextScene());
    }

    private IEnumerator CloseDoorsAndLoadNextScene()
    {
        // 문 애니메이션이 끝날 때까지 정확히 대기
        yield return StartCoroutine(activateDoors());

        asyncLoad = SceneManager.LoadSceneAsync(SceneTransitionManager.loadSceneName);
        asyncLoad.allowSceneActivation = false;
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }

    /// <summary>
    /// 애니메이션이 끝날때 까지 대기하는 코루틴
    /// </summary>
    private IEnumerator WaitForDoorsToFinish()
    {
        while (!leftDoor.isFinished() || !rightDoor.isFinished())
        {
            yield return null;
        }
    }
    /// <summary>
    /// 애니메이션 작동.
    /// </summary>
    /// <returns></returns>
    private IEnumerator activateDoors()
    {
        leftDoor.onclicked();
        rightDoor.onclicked();

        // 문 애니메이션이 끝날 때까지 정확히 대기
        yield return StartCoroutine(WaitForDoorsToFinish());

        capturedImage.gameObject.SetActive(false);
    }
}
