using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SonaExpantion : MonoBehaviour
{
    private GameObject sona = null;
    Light2D sona_light = null;
    SpriteRenderer spriteRenderer = null;
    Transform spriteTransform = null;
    Transform lightTransform = null;

    public Vector3 sonaScale = new Vector3(3f, 3f, 3f);
    // inspector :
    // extandeTime 0.1
    // fadeTime 0.3
    public float extandeTime;
    public float fadeTime;
    public float lightFadeTime;

    public float initialOuterRadius = 1.0f;

    void Start()
    {
        sona = this.gameObject;

        // 자식 오브젝트에서 컴포넌트를 가져옵니다.
        spriteRenderer = sona.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        spriteTransform = sona.transform.Find("Sprite").GetComponent<Transform>();

        sona_light = sona.transform.Find("Light2D").GetComponent<Light2D>();
        lightTransform = sona.transform.Find("Light2D").GetComponent<Transform>();

        StartCoroutine(LightExpand());
        StartCoroutine(ExpandAndFade());
    }

    IEnumerator ExpandAndFade()
    {
        Vector3 initialScale = spriteTransform.localScale;
        Vector3 targetScale = sonaScale;
        float elapsedTime = 0f;

        // Expand
        while (elapsedTime < extandeTime)
        {
            spriteTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / extandeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteTransform.localScale = targetScale;

        // Fade
        elapsedTime = 0;
        Color initialColor = spriteRenderer.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0);

        while (elapsedTime < fadeTime)
        {
            spriteRenderer.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = targetColor;
    }

    IEnumerator LightExpand()
    {
        float targetRadius = initialOuterRadius * 2f;
        float lightTime = 0f;

        sona_light.pointLightOuterRadius = 2.0f;

        // Expand
        while (lightTime < lightFadeTime)
        {
            sona_light.pointLightOuterRadius = Mathf.Lerp(targetRadius, initialOuterRadius, lightTime / lightFadeTime);
            lightTime += Time.deltaTime;
            yield return null;
        }
    }
}
