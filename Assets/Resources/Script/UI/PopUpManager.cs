using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PopUpManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static PopUpManager Instance { get; private set; }

    [Header("Popup Settings")]
    // �˾� ������ (Popup ������Ʈ�� ���ԵǾ� �־�� ��)
    public GameObject popupPrefab;

    // �˾��� ���� ĵ����(�Ǵ� �θ� Transform). Inspector���� �Ҵ�.
    public Transform popupCanvas;

    // ���� Ȱ��ȭ�Ǿ� �ִ� �˾� �ν��Ͻ� (����)
    private GameObject popupInstance;

    private void Awake()
    {
        // �̱��� �ʱ�ȭ
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
    /// ������ ���Կ��� ȣ���ϸ�, �ش� �������� ������ ���� �˾��� ǥ���մϴ�.
    /// </summary>
    /// <param name="itemData">ǥ���� ������ ������ (��: Item Ÿ�� �Ǵ� ItemData Ÿ��)</param>
    public void ShowPopup(int popupid, ItemData itemdata)
    {
        if (itemdata == null) return;

        // �˾��� ó�� ȣ��Ǹ� �ν��Ͻ� ����
        if (popupInstance == null)
        {
            // false �ɼ����� Instantiate�ϸ�, �������� ���� RectTransform ����(��Ŀ, �ǹ� ��)�� �����˴ϴ�.
            popupInstance = Instantiate(popupPrefab, popupCanvas, false);
        }
        else
        {
            // �̹� �����ϸ� Ȱ��ȭ
            popupInstance.SetActive(true);
        }

        // �˾� ������Ʈ�� Initialize �޼��带 ȣ���Ͽ� ������ ������ �����մϴ�.
        updatePopup popupComponent = popupInstance.GetComponent<updatePopup>();
        if (popupComponent != null)
        {
            popupComponent.loadItem(popupid, itemdata);
        }
        else
        {
            Debug.LogWarning("Popup ������Ʈ�� ã�� �� �����ϴ�. �˾� �����տ� Popup ��ũ��Ʈ�� Ȯ���ϼ���.");
        }
    }

    /// <summary>
    /// �˾��� ����ϴ�.
    /// </summary>
    public void HidePopup()
    {
        if (popupInstance != null)
        {
            popupInstance.SetActive(false);
        }
    }
}
