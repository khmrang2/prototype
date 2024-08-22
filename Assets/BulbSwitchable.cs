using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BulbSwitchable : MonoBehaviour
{
    private Light2D light2D;
    private Coroutine blinkCoroutine;
    public SpriteRenderer spriteRenderer;
    public Sprite sprite_on;
    public Sprite sprite_off;

    public float blinkInterval = 1f; // �����Ÿ��� ����
    public float activeDuration = 5f; // ���� ���¸� �����ϴ� �ð�

    public float minBlinkInterval = 0.5f; // �ּ� �����Ÿ� ���� (��)
    public float maxBlinkInterval = 2f; // �ִ� �����Ÿ� ���� (��)
    public float minBlinkDuration = 0.05f; // �ּ� �����Ÿ� ���� �ð� (��)
    public float maxBlinkDuration = 0.2f; // �ִ� �����Ÿ� ���� �ð� (��)
    public float minIntensity = 0f; // �ּ� ��� (���� ����)
    public float maxIntensity = 1f; // �ִ� ��� (���� ����)

    void Start()
    {
        light2D = GetComponent<Light2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();   
        spriteRenderer.sprite = sprite_off; // �ʱ⿡�� ���� ����
        light2D.intensity = 0f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spriteRenderer.sprite = sprite_on; // ��������Ʈ�� ���� ���·� ����
            light2D.intensity = 1f; // ������ ���� ���·� ����

            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine); // ���� �ڷ�ƾ�� ������ ����
            }

            blinkCoroutine = StartCoroutine(BlinkAndStopCoroutine());
        }
    }

    IEnumerator BlinkAndStopCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < activeDuration) { 
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

        // �ð��� �� �Ǹ� ������ ���� ��������Ʈ�� ���� ���·� ����
        light2D.intensity = 0f;
        spriteRenderer.sprite = sprite_off;
        blinkCoroutine = null;
    }
}
