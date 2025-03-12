using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FontSizeAdjuster : MonoBehaviour
{
    public TextMeshProUGUI titleText; // Auto Size 켜진 제목 텍스트
    public List<TextMeshProUGUI> subTexts;   // 비율로 맞출 텍스트
    [Range(0.1f, 1f)]
    public float ratio = 0.8f; // 제목 폰트 크기의 80%로 설정

    public void adjustFont()
    {
        // 레이아웃 및 폰트 사이즈 계산이 완료될 때까지 기다린 후 실행
        StartCoroutine(AdjustSubTextSize());
    }

    IEnumerator AdjustSubTextSize()
    {
        // 한 프레임 대기 (또는 WaitForEndOfFrame)하여 Auto Size가 적용되도록 함.
        yield return new WaitForEndOfFrame();
        // 강제로 메쉬 업데이트를 시켜서 최종 폰트 사이즈가 반영되도록 함.
        titleText.ForceMeshUpdate();

        // 제목 텍스트의 실제 계산된 폰트 크기를 가져옴.
        float computedFontSize = titleText.fontSize;
        
        // 오토사이즈된 친구의 일정 비율로 폰트 크기 조절.
        foreach(TextMeshProUGUI subText in subTexts)
        {
            subText.enableAutoSizing = false;
            subText.fontSize = computedFontSize * ratio;
        }
    }
}