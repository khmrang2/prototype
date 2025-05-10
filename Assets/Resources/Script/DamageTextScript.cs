using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DamageTextScript : MonoBehaviour
{

    private TextMeshPro tmp;
    private Coroutine fadeCoroutine;
    private RectTransform rect;
    private Vector3 startPos;

    public float duration = 1f;
    public float holdTime = 0.3f;
    public float moveUpAmount = 0.5f;
   


    void Start()
    {
        tmp = GetComponent<TextMeshPro>();
        startPos = transform.position;
        fadeCoroutine = StartCoroutine(FadeOut());
    }



    IEnumerator FadeOut()
    {
        Color color = tmp.color;
        float startAlpha = color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // 일시정지 중이면 멈춤, 없어도 투명해지지는 않는데 대신 연산량이 증가
            if (Time.deltaTime == 0f)
            {
                yield return null;
                continue;
            }

            elapsed += Time.deltaTime;

            float normalized = Mathf.Clamp01(elapsed / duration);

            if (elapsed > holdTime)
            {
                color.a = Mathf.Lerp(startAlpha, 0f, normalized);
                tmp.color = color;
            }

            transform.position = Vector3.Lerp(startPos, startPos + Vector3.up * moveUpAmount, normalized);

            yield return null;
        }

        Destroy(this.gameObject);

    }

 
}
