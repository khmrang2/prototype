using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FontSizeAdjuster : MonoBehaviour
{
    public TextMeshProUGUI titleText; // Auto Size ���� ���� �ؽ�Ʈ
    public List<TextMeshProUGUI> subTexts;   // ������ ���� �ؽ�Ʈ
    [Range(0.1f, 1f)]
    public float ratio = 0.8f; // ���� ��Ʈ ũ���� 80%�� ����

    public void adjustFont()
    {
        // ���̾ƿ� �� ��Ʈ ������ ����� �Ϸ�� ������ ��ٸ� �� ����
        StartCoroutine(AdjustSubTextSize());
    }

    IEnumerator AdjustSubTextSize()
    {
        // �� ������ ��� (�Ǵ� WaitForEndOfFrame)�Ͽ� Auto Size�� ����ǵ��� ��.
        yield return new WaitForEndOfFrame();
        // ������ �޽� ������Ʈ�� ���Ѽ� ���� ��Ʈ ����� �ݿ��ǵ��� ��.
        titleText.ForceMeshUpdate();

        // ���� �ؽ�Ʈ�� ���� ���� ��Ʈ ũ�⸦ ������.
        float computedFontSize = titleText.fontSize;
        
        // ���������� ģ���� ���� ������ ��Ʈ ũ�� ����.
        foreach(TextMeshProUGUI subText in subTexts)
        {
            subText.enableAutoSizing = false;
            subText.fontSize = computedFontSize * ratio;
        }
    }
}