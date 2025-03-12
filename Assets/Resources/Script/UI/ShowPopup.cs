using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowPopup : MonoBehaviour
{
    public GameObject popUpPrefab; // 활성화할 프리팹 팝업
    public updatePopup popUpScript; // 활성화할 팝업 코드
    public GameObject canvas; // 팝업창을 그릴 캔버스.

    private GameObject popUpInstance; // 활성화할 프리팹 팝업.

    // 팝업창을 생성시 자신의 아이콘을 넘겨줘야 함.

    // 버튼 클릭 시 호출되는 함수
    public void ActivatePrefab()
    {
        if (popUpInstance == null)
        {
            // 팝업 캔버스를 부모로 하여 팝업을 생성, 프리팹의 RectTransform 설정이 유지됨.
            popUpInstance = Instantiate(popUpPrefab, canvas.transform, false);
        }
        else
        {
            popUpInstance.SetActive(true);
        }
        popUpScript = popUpPrefab.GetComponent<updatePopup>();
    }
}
