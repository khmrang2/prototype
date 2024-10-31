using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightBlink : MonoBehaviour
{
    private Light2D light2D;

    public float minBlinkInterval = 0.5f; // �ּ� �����Ÿ� ���� (��)
    public float maxBlinkInterval = 2f; // �ִ� �����Ÿ� ���� (��)
    public float minBlinkDuration = 0.05f; // �ּ� �����Ÿ� ���� �ð� (��)
    public float maxBlinkDuration = 0.2f; // �ִ� �����Ÿ� ���� �ð� (��)
    public float minIntensity = 0f; // �ּ� ��� (���� ����)
    public float maxIntensity = 1f; // �ִ� ��� (���� ����)

    void Start()
    {
        light2D = GetComponent<Light2D>();
        StartCoroutine(BlinkCoroutine());
    }

    IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            // �����Ÿ��� ���� �ð��� �������� ����
            float blinkDuration = Random.Range(minBlinkDuration, maxBlinkDuration);

            // ���� ���·� ��ȯ
            light2D.intensity = minIntensity;
            yield return new WaitForSeconds(blinkDuration);

            // �����Ÿ��� ������ �������� ����
            float blinkInterval = Random.Range(minBlinkInterval, maxBlinkInterval);

            // ���� ���·� ��ȯ
            light2D.intensity = maxIntensity;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}