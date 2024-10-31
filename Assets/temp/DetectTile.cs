using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DetectTile : MonoBehaviour
{
    public Color newColor;
    SpriteRenderer spriteRenderer = null;
    public float fadeTime = 0.1f;


    private void Start()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Tile detected");
        StartCoroutine(FadeOut());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("detect initialized");
        StartCoroutine (FadeIn());
    }


    IEnumerator FadeOut()
    {
        float time = 0;
        Color initialColor = spriteRenderer.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0);

        while (time < fadeTime)
        {
            spriteRenderer.color = Color.Lerp(initialColor, targetColor, time / fadeTime);
            time += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = targetColor;
    }

    IEnumerator FadeIn()
    {
        float time = 0;
        Color initialColor = spriteRenderer.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1);

        while (time < fadeTime)
        {
            spriteRenderer.color = Color.Lerp(initialColor, targetColor, time / fadeTime);
            time += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = targetColor;
    }

}
