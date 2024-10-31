using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GoalAction : MonoBehaviour
{
    float initialRadius;
    Light2D goal_light;
    SpriteRenderer background;
    public GameDirector gameManager;
    public float endTime = 3f;
    private bool endC1 = false;
    private bool endC2 = false;
    // Start is called before the first frame update
    void Start()
    {
        goal_light = GetComponent<Light2D>();
        background = transform.parent.Find("Background").GetComponent<SpriteRenderer>();
        initialRadius = goal_light.pointLightOuterRadius;
    }
    private void Update()
    {
        if(endC1 && endC2)
        {
            gameManager.goaled = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))   
        {
            Debug.Log("ÇÃ·¹ÀÌ¾î°¡ °ñ¿¡ µµÂø");
            StartCoroutine(GoalRoutine());
            StartCoroutine(GlobalRoutine());
        }
    }
    IEnumerator GlobalRoutine()
    {
        float gradationTime = 0f;

        goal_light.pointLightOuterRadius = 2.0f;

        // Expand
        while (gradationTime < endTime)
        {
            float a = Mathf.Lerp(0, 1, gradationTime / (endTime/2));
            background.color = new Color(a, a, a);
            gradationTime += Time.deltaTime;
            yield return null;
        }
        endC1 = true;
    }

    IEnumerator GoalRoutine()
    {
        float targetRadius = 100f;
        float lightTime = 0f;

        goal_light.pointLightOuterRadius = 2.0f;

        // Expand
        while (lightTime < endTime)
        {
            goal_light.pointLightOuterRadius = Mathf.Lerp(initialRadius, targetRadius, lightTime / endTime);
            lightTime += Time.deltaTime;
            yield return null;
        }
        endC2 = true;
    }
}

