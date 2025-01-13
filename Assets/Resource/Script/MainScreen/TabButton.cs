using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    [SerializeField] private RectTransform icon;            // 아이콘 이미지

    [SerializeField] private Image background;              // 기본 배경 이미지
    [SerializeField] private Sprite active_background;       // 활성화 배경 이미지
    [SerializeField] private Sprite deactive_background;     // 비활성화 배경 이미지

    [SerializeField] private Vector3 activePositionOffset;  // 활성화 위치 이동량.
    private Vector3 originalPosition;                       // 초기 아이콘 위치

    private void Start()
    {
        // 초기 위치 저장
        if (icon != null)
            originalPosition = icon.localPosition;
    }

    // Activate() :
    // 버튼이 활성화 되었을 때
    // 1. -> local position 찾아가기.
    // 2. 탭 배경 색상 -> activated가기.
    public void Activate()
    {
        if (background != null)
        {
            // 색상 변경
            background.sprite = active_background;
            // icon 위치 변경
            icon.localPosition = originalPosition + activePositionOffset;
        }
    }

    // 비활성화 되었을때
    // 1. -> local position 찾아가기.
    // 2. 탭 배경 색상 -> deactivated가기.
    public void Deactivate()
    {
        if (background != null)
        {
            // 색상 변경
            background.sprite = deactive_background;
            // icon 위치 변경
            icon.localPosition = originalPosition;
        }
    }
}
