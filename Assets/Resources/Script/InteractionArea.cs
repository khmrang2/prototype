using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerState playerState;
    [SerializeField] private GameObject ballPrefab; // 공 프리팹
    [SerializeField] private GameObject dropParticle; //파티클 프리팹
    [SerializeField] private float spawnOffset = 0.1f; // 누른 위치에서 offset만큼 떨어진 거리에서 공 스폰.
    [SerializeField] private RectTransform interactionArea;
    [SerializeField] private RectTransform DropArea; // interaction area Transform

    private int numberOfBalls = 3; // 디폴트 3 : PlayerState로 변경.
    private bool isReadyToDrop = false; // 공 생성 여부
    private GameObject activeParticle;

    void Start()
    {
        numberOfBalls = playerState.Ball_Count;
    }

    // 눌렀을때
    public void OnPointerDown(PointerEventData eventData)
    {
        isReadyToDrop = true;

        // 파티클 초기 생성 위치 계산
        float mappedX = GetMappedXFromPointer(eventData);
        Vector3 particlePos = new Vector3(mappedX, DropArea.position.y, 0f);

        activeParticle = Instantiate(dropParticle, particlePos, dropParticle.transform.rotation);
    }

    void Update()
    {
        if (isReadyToDrop && activeParticle != null)
        {
#if UNITY_EDITOR
            Vector3 pointerPosition = Input.mousePosition;
#else
            if (Input.touchCount == 0) return;
            Vector3 pointerPosition = Input.touches[0].position;
#endif
            float mappedX = GetMappedXFromScreenX(pointerPosition.x);
            Vector3 newPos = new Vector3(mappedX, DropArea.position.y, 0f);
            activeParticle.transform.position = newPos;
        }
    }

    // 누르고 뗐을때 공이 떨어짐.
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isReadyToDrop && numberOfBalls > 0)
        {
            float mappedX = GetMappedXFromPointer(eventData);
            Vector3 spawnPosition = new Vector3(mappedX, DropArea.position.y, 0f);

            for (; numberOfBalls > 0; numberOfBalls--)
            {
                Vector3 offset = new Vector3(Random.Range(-spawnOffset, spawnOffset), Random.Range(-spawnOffset, spawnOffset), 0f);

                GameObject newBall = Instantiate(ballPrefab, spawnPosition + offset, Quaternion.identity);
                newBall.GetComponent<Ball>().wasSplited = false;
            }

            if (activeParticle != null)
                Destroy(activeParticle);

            isReadyToDrop = false;
        }
    }

    public void init_ball(){
        numberOfBalls = playerState.Ball_Count;
    }
    public int get_ball_num(){
        return numberOfBalls;
    }

    #region --- 🔽 Helper Functions 🔽 ---

    private float GetMappedXFromPointer(PointerEventData eventData)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0;
        return MapToDropAreaX(worldPos.x);
    }

    private float GetMappedXFromScreenX(float screenX)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenX, 0, Camera.main.nearClipPlane));
        worldPos.z = 0;
        return MapToDropAreaX(worldPos.x);
    }

    private float MapToDropAreaX(float inputX)
    {
        float interMinX = GetWorldMinX(interactionArea);
        float interMaxX = GetWorldMaxX(interactionArea);
        float dropMinX = GetWorldMinX(DropArea);
        float dropMaxX = GetWorldMaxX(DropArea);

        float percent = Mathf.InverseLerp(interMinX, interMaxX, inputX);
        float mappedX = Mathf.Lerp(dropMinX, dropMaxX, percent);

        return mappedX;
    }

    private float GetWorldMinX(RectTransform rect)
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        return corners[0].x;
    }

    private float GetWorldMaxX(RectTransform rect)
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        return corners[3].x;
    }
    #endregion
}