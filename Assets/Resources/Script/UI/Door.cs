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
    public float animationDuration = 1.5f; // �ִϸ��̼� �ð�

    void Start()
    {
        coroutineSpinLock = true;
        rectTransform = GetComponent<RectTransform>();

        // ���� UI�� ���� ��ġ ����
        Vector2 originalPosition = rectTransform.anchoredPosition;
        rectTransform.pivot = pos;

        // ��ġ ����
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
    /// ���ɶ� ��ȯ��.
    /// </summary>
    /// <returns></returns>
    public bool isFinished()
    {
        return coroutineSpinLock;
    }

    // ���� �ִϸ��̼ǵ��� �������ִ� �޼ҵ�.
    // ���ɶ�(coroutineSpinLock)���� �ִϸ��̼� ���൵�� ���� �ٲ��� �ʵ��� ��ȯ ������ ��������.
    public void onclicked()
    {
        if (!coroutineSpinLock) return; // �̹� ���� ���̸� �ߺ� ���� ����
        coroutineSpinLock = false; // ���� �� �÷��� ����

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
    /// �ڷ�ƾ
    /// ���� ����.
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
        coroutineSpinLock = true; // �ִϸ��̼� �Ϸ� �� �÷��� ����
    }

    /// <summary>
    /// ���� ����
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
        coroutineSpinLock = true; // �ִϸ��̼� �Ϸ� �� �÷��� ����
    }
    //private IEnumerator FadeOut()
    //{
    //    float elapsedTime = 0f;
    //    Color startColor = doorImage.color; // ���� �� ���� ��������

    //    while (elapsedTime < fadeDuration)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        float t = elapsedTime / fadeDuration;

    //        // ���� ���������� ���� (RGB ����)
    //        doorImage.color = Color.Lerp(startColor, Color.black, t);
    //        yield return null;
    //    }
    //    // ������ ���������� ����
    //    doorImage.color = Color.black;
    //    coroutineSpinLock = true;
    //}

}
