using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Door : MonoBehaviour
{

    private bool coroutineSpinLock;
    [SerializeField]
    public bool opened;
    // Start is called before the first frame update
    private RectTransform rectTransform;
    [SerializeField]
    private Vector2 pos;

    [SerializeField]
    private GameObject myself;
    public float animationDuration = 1.5f; // 애니메이션 시간

    void Start()
    {
        coroutineSpinLock = true;
        rectTransform = GetComponent<RectTransform>();

        // 현재 UI의 원래 위치 저장
        Vector2 originalPosition = rectTransform.anchoredPosition;
        rectTransform.pivot = pos;

        // 위치 보정
        rectTransform.anchoredPosition = originalPosition;
    }

    public void setActive()
    {
        myself.SetActive(true);
    }

    public void setFalse()
    {
        myself.SetActive(false);
    }

    /// <summary>
    /// 스핀락 반환자.
    /// </summary>
    /// <returns></returns>
    public bool isFinished()
    {
        return coroutineSpinLock;
    }

    // 대충 애니메이션들을 시작해주는 메소드.
    // 스핀락(coroutineSpinLock)으로 애니메이션 실행도중 씬이 바뀌지 않도록 전환 순서를 강제해줌.
    public void onclicked()
    {
        if (!coroutineSpinLock) return; // 이미 실행 중이면 중복 실행 방지
        coroutineSpinLock = false; // 실행 중 플래그 설정

        switch (opened)
        {
            case true:
                StartCoroutine(closeDoor());
                break;
            case false:
                StartCoroutine(openDoor());
                break;
        };
            
    }

    /// <summary>
    /// 코루틴
    /// 문을 닫자.
    /// </summary>
    /// <returns></returns>
    private IEnumerator closeDoor()
    {
        for (float i = -180f; i <= 0f; i += 10f)
        {
            transform.rotation = Quaternion.Euler(0f, i, 0f);
            yield return new WaitForSeconds(0.03f);
        }
        opened = false;
        yield return new WaitForSeconds(1f);
        coroutineSpinLock = true; // 애니메이션 완료 후 플래그 해제
    }

    /// <summary>
    /// 문을 열자
    /// </summary>
    /// <returns></returns>
    private IEnumerator openDoor()
    {
        for (float i = 0f; i >= -180f; i -= 10f)
        {
            transform.rotation = Quaternion.Euler(0f, i, 0f);
            yield return new WaitForSeconds(0.03f);
        }

        opened = true;
        yield return new WaitForSeconds(1f);
        coroutineSpinLock = true; // 애니메이션 완료 후 플래그 해제
    }
    //private IEnumerator FadeOut()
    //{
    //    float elapsedTime = 0f;
    //    Color startColor = doorImage.color; // 현재 문 색상 가져오기

    //    while (elapsedTime < fadeDuration)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        float t = elapsedTime / fadeDuration;

    //        // 점점 검은색으로 변경 (RGB 감소)
    //        doorImage.color = Color.Lerp(startColor, Color.black, t);
    //        yield return null;
    //    }
    //    // 완전히 검은색으로 변경
    //    doorImage.color = Color.black;
    //    coroutineSpinLock = true;
    //}

}
