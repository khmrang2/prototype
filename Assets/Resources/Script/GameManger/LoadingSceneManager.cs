using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager;

public class LoadingSceneManager : MonoBehaviour
{
    public Door leftDoor, rightDoor;
    public RawImage capturedImage; // ���� ������ �ҷ��� ù ������.
    public float transitionDuration = 1.5f;

    private AsyncOperation asyncLoad;
    private RenderTexture sceneRenderTexture; // �� ù �������� ������ RenderTexture

    void Start()
    {
        if (SceneTransitionManager.lastScreenTexture != null)
        {
            capturedImage.texture = SceneTransitionManager.lastScreenTexture;
            capturedImage.gameObject.SetActive(true);
            capturedImage.rectTransform.localScale = new Vector3(1, -1, 1); // �̹��� ���� �ذ�
        }
        StartCoroutine(CloseDoorsAndLoadNextScene());
    }

    private IEnumerator CloseDoorsAndLoadNextScene()
    {
        // �� �ִϸ��̼��� ���� ������ ��Ȯ�� ���
        yield return StartCoroutine(activateDoors());

        asyncLoad = SceneManager.LoadSceneAsync(SceneTransitionManager.loadSceneName);
        asyncLoad.allowSceneActivation = false;

        Debug.LogError("�� ���� ����!");
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }
        Debug.LogError("�� ���� ���� ����!");
        Debug.LogError("�� ��ȯ ����!");
        asyncLoad.allowSceneActivation = true;
    }

    /// <summary>
    /// �ִϸ��̼��� ������ ���� ����ϴ� �ڷ�ƾ
    /// </summary>
    private IEnumerator WaitForDoorsToFinish()
    {
        while (!leftDoor.isFinished() || !rightDoor.isFinished())
        {
            yield return null;
        }
    }
    /// <summary>
    /// �ִϸ��̼� �۵�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator activateDoors()
    {
        leftDoor.onclicked();
        rightDoor.onclicked();

        // �� �ִϸ��̼��� ���� ������ ��Ȯ�� ���
        yield return StartCoroutine(WaitForDoorsToFinish());

        capturedImage.gameObject.SetActive(false);
    }
}
