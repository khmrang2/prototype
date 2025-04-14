using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
//using UnityEditor.PackageManager;

public class LoadingSceneManager : MonoBehaviour
{
    public Door leftDoor, rightDoor;
    public RawImage capturedImage; // 이전 씬에서 불러온 첫 프레임.
    public float transitionDuration = 1.5f;

    [SerializeField] private GameObject popup_error;
    [SerializeField] private DataControl datactr;
    [SerializeField] private TextMeshProUGUI loadingtext;

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
        bool success = false;
        bool isDone = false;

        Debug.Log("문 닫히는 애니메이션 시작!");
        // 1. 문 애니메이션이 끝날 때까지 정확히 대기
        yield return StartCoroutine(activateDoors());

        Debug.Log("문 닫기 애니메이션 종료!");
        loadingtext.gameObject.SetActive(true);

        // 2. 데이터 한번 저장하기.
        datactr.SaveDataWithCallback(result => {
            success = result;
            isDone = true;
        });

        // 3. 씬로드 = 데이터 저장은 병렬로 진행하니까..
        Debug.Log("씬 로딩하기!");
        asyncLoad = SceneManager.LoadSceneAsync(SceneTransitionManager.loadSceneName);
        asyncLoad.allowSceneActivation = false;

        // 4. 데이터 저장 성공시 이후 로직 진행 X 기다리기.
        yield return new WaitUntil(() => isDone);
        // 4-1. 데이터 저장 실패 -> 종료 팝업
        if (!success)
        {
            popup_error.SetActive(true);
            yield break; // 이후 코드 진행되지 않음
        }

        // 4-2. 데이터 저장 성공시
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }
        Debug.Log("씬 전환하기!");


        loadingtext.gameObject.SetActive(false);
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
