using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncSceneLoader : MonoBehaviour
{
    public void LoadSceneAsync(string sceneName)
    {
        // 비동기로 Scene을 로드
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    /* LoadSceneCoroutine
     * 코루틴으로 씬을 불러오는 함수.
     * 
     * 씬을 부르면서, 파일 입출력, 변수 전달을 같이 하기 위해서.
     * 즉, 멀티쓰레딩 같은 효과를 주기 위해서 이와같은 함수를 사용했다.
     **/
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // 비동기로 씬을 불러올 객체 생성.
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // 로딩이 완료될 때까지 기다림
        while (!operation.isDone)
        {
            // 진행률 확인 가능 (0.0 ~ 1.0)
            Debug.Log("Loading progress: " + operation.progress);

            yield return null; // 다음 프레임까지 기다림
        }
    }
}