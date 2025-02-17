using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncSceneLoader : MonoBehaviour
{
    [SerializeField]
    public Animator animator;
    /** 씬 이름 정리
     * MainScreen :     메인 씬
     * PlayScreen :   플레이 씬
     * LoadingScreen :  로딩 씬
     * 
     */

    public void LoadSceneAsync(string sceneName)
    {
        // 비동기로 Scene을 로드
        // SceneName은 해당 버튼의 매개변수로 할당함.
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
        //// 애니메이터가 존재하는 경우에만 실행.
        //if(animator != null)
        //{
        //    // 애니메이션 A 실행 (1회)
        //    animator.Play("AnimationA");
        //    yield return new WaitForSeconds(2f); // 애니메이션 연출 시간

        //    // 애니메이션 B 실행 (반복)
        //    animator.Play("AnimationB");
        //}

        // 게임 씬 비동기 로드 시작
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // 자동 씬 전환 방지

        // 로딩 진행률 확인 (0.9 이상이면 로딩 거의 완료)
        while (asyncLoad.progress < 0.9f)
        {
            Debug.Log("씬 로딩 완료, 게임 씬 준비 신호 대기 중...");
            yield return null;
        }

        //if(animator != null)
        //{
        //    // 애니메이션 C 실행 (반복)
        //    animator.Play("AnimationC");
        //}

        Debug.Log("게임 씬 준비 완료! 씬 전환 진행");
        yield return new WaitForSeconds(1f); // 추가 연출 시간

        asyncLoad.allowSceneActivation = true; // 게임 씬으로 이동
    }
}