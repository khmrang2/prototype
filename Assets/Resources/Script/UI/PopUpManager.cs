using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PopUpManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static PopUpManager Instance { get; private set; }

    [Header("Popup Settings")]
    // 팝업 프리팹 (Popup 컴포넌트가 포함되어 있어야 함)
    public GameObject popupPrefab;

    // 팝업이 속할 캔버스(또는 부모 Transform). Inspector에서 할당.
    public Transform popupCanvas;

    // 현재 활성화되어 있는 팝업 인스턴스 (재사용)
    private GameObject popupInstance;

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 아이템 슬롯에서 호출하면, 해당 아이템의 정보를 담은 팝업을 표시합니다.
    /// </summary>
    /// <param name="itemData">표시할 아이템 데이터 (예: Item 타입 또는 ItemData 타입)</param>
    public void ShowPopup(int popupid, ItemData itemdata)
    {
        if (itemdata == null) return;

        // 팝업이 처음 호출되면 인스턴스 생성
        if (popupInstance == null)
        {
            // false 옵션으로 Instantiate하면, 프리팹의 로컬 RectTransform 설정(앵커, 피벗 등)이 유지됩니다.
            popupInstance = Instantiate(popupPrefab, popupCanvas, false);
        }
        else
        {
            // 이미 존재하면 활성화
            popupInstance.SetActive(true);
        }

        // 팝업 컴포넌트의 Initialize 메서드를 호출하여 아이템 정보를 전달합니다.
        updatePopup popupComponent = popupInstance.GetComponent<updatePopup>();
        if (popupComponent != null)
        {
            popupComponent.loadItem(popupid, itemdata);
        }
        else
        {
            Debug.LogWarning("Popup 컴포넌트를 찾을 수 없습니다. 팝업 프리팹에 Popup 스크립트를 확인하세요.");
        }
    }

    /// <summary>
    /// 팝업을 숨깁니다.
    /// </summary>
    public void HidePopup()
    {
        if (popupInstance != null)
        {
            popupInstance.SetActive(false);
        }
    }
}
