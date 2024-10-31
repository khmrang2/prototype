using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightBlink : MonoBehaviour
{
    private Light2D light2D;

    public float minBlinkInterval = 0.5f; // 최소 깜빡거림 간격 (초)
    public float maxBlinkInterval = 2f; // 최대 깜빡거림 간격 (초)
    public float minBlinkDuration = 0.05f; // 최소 깜빡거림 지속 시간 (초)
    public float maxBlinkDuration = 0.2f; // 최대 깜빡거림 지속 시간 (초)
    public float minIntensity = 0f; // 최소 밝기 (꺼짐 상태)
    public float maxIntensity = 1f; // 최대 밝기 (켜짐 상태)

    void Start()
    {
        light2D = GetComponent<Light2D>();
        StartCoroutine(BlinkCoroutine());
    }

    IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            // 깜빡거리는 지속 시간을 랜덤으로 설정
            float blinkDuration = Random.Range(minBlinkDuration, maxBlinkDuration);

            // 꺼짐 상태로 전환
            light2D.intensity = minIntensity;
            yield return new WaitForSeconds(blinkDuration);

            // 깜빡거리는 간격을 랜덤으로 설정
            float blinkInterval = Random.Range(minBlinkInterval, maxBlinkInterval);

            // 켜짐 상태로 전환
            light2D.intensity = maxIntensity;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}