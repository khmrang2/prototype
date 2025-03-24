using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterHitted : MonoBehaviour
{
    public SpriteRenderer[] spriteRenderers;
    private Color[] originalColors;
    [Header("피격 플래시의 지속 시간")]
    public float flashDuration = 0.2f;

    // 이벤트 생성 (인스펙터에서 연결 가능)
    public UnityEvent onDamageTaken;

    void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        originalColors = new Color[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalColors[i] = spriteRenderers[i].color;
        }
    }

    // 이 함수를 호출하여 실행.
    public void TakeDamage()
    {
        StartCoroutine(FlashRed());
        onDamageTaken?.Invoke(); // 이벤트 호출
    }

    /// <summary>
    /// 공격 당함 애니메이션 - 코루틴으로 작성. 
    /// flashDuration만큼 실행됨.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FlashRed()
    {
        // 덜 자극적인 붉은 색 지정
        Color flashColor = new Color(1f, 0.5f, 0.5f, 1f);

        foreach (var sr in spriteRenderers)
        {
            sr.color = flashColor;
        }

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = originalColors[i];
        }
    }
}
