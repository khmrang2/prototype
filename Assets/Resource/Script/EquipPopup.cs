using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipPopup : MonoBehaviour
{
    public GameObject popUpPanel; // 활성화할 프리팹
    private GameObject spawnedPopUp; // 생성된 오브젝트를 참조할 변수

    // 버튼 클릭 시 호출되는 함수
    public void ActivatePrefab()
    {
        if (popUpPanel == null)
        {
            Debug.LogWarning("Prefab is not assigned!");
            return;
        }

        popUpPanel.SetActive(true);
    }
}
