using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemDrops : MonoBehaviour
{
    public Image coinPrefab;
    public Image experiencePrefab;
    public Image potionPrefab;
    
    private Transform playerTransform;
    private RectTransform goldUIPosition;
    
    // 아이템 획득 완료 이벤트
    public delegate void ItemCollectedHandler();
    public event ItemCollectedHandler OnItemCollected;
    
    private Camera mainCamera;
    private Canvas mainCanvas;
    
    private Vector2 playerScreenPosition;
    private Vector2 playerCanvasPosition;

    [SerializeField] private float duration = 1.0f;

    private void Awake()
    {
        // 플레이어 
        playerTransform = GameObject.FindGameObjectWithTag("player")?.transform;
        if (!playerTransform)
        {
            Debug.LogWarning("DropItem: Player not found!");
        }
        
        // GoldUI - rectTransform 
        GameObject goldUIObject = GameObject.FindGameObjectWithTag("GoldUI");
        if (goldUIObject != null)
        {
            goldUIPosition = goldUIObject.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogWarning("DropItem: GoldUI not found!");
        }
        
        mainCamera = Camera.main;
        if (mainCamera == null) Debug.LogError("DropItem: Main camera not found!");

        // mainCanvas : 부모 오브젝트 중 Canvas 컴포넌트
        mainCanvas = GetComponentInParent<Canvas>();
        if (mainCanvas == null)
        {
            Debug.LogError("ItemDrops: Canvas not found in parent hierarchy!");
        }
        else
        {
            Debug.Log($"ItemDrops: Found Canvas - {mainCanvas.name}");
        }
    }
    
    private void OnEnable()
    {
        if (playerTransform != null && mainCamera != null)
        {
            UpdatePlayerPosition();
            UpdateExpPosition();
            ComparePositions(); 
        }
        else
        {
            Debug.LogError("OnEnable - Required components are missing for position initialization.");
        }
    }
    
    private void ComparePositions()
    {
        if (playerTransform == null || experiencePrefab == null || mainCamera == null)
        {
            Debug.LogError("ComparePositions: Required components are missing.");
            return;
        }

        // Player의 월드 위치
        Vector3 playerWorldPos = playerTransform.position;

        // EXP의 월드 위치 (UI 요소이므로 변환 필요)
        Vector3 expWorldPos = mainCamera.ScreenToWorldPoint(experiencePrefab.transform.position);
        expWorldPos.z = playerWorldPos.z; // z 좌표를 player와 동일하게 설정

        Debug.Log($"Player World Position: {playerWorldPos}");
        Debug.Log($"EXP World Position: {expWorldPos}");

        // 두 위치 간의 차이 계산
        Vector3 difference = expWorldPos - playerWorldPos;
        Debug.Log($"Difference (EXP - Player): {difference}");

        // 화면상의 위치 비교
        Vector2 playerScreenPos = mainCamera.WorldToScreenPoint(playerWorldPos);
        Vector2 expScreenPos = mainCamera.WorldToScreenPoint(expWorldPos);

        Debug.Log($"Player Screen Position: {playerScreenPos}");
        Debug.Log($"EXP Screen Position: {expScreenPos}");

        // 화면상 위치 차이
        Vector2 screenDifference = expScreenPos - playerScreenPos;
        Debug.Log($"Screen Difference (EXP - Player): {screenDifference}");
    }

    private void UpdatePlayerPosition()
    {
        // 1. World Position
        Vector3 playerWorldPos = playerTransform.position;
        Debug.Log($"Player World Position: {playerWorldPos}");

        // 2. Screen Position
        Vector2 playerScreenPos = mainCamera.WorldToScreenPoint(playerWorldPos);
        Debug.Log($"Player Screen Position: {playerScreenPos}");

        // 3. Viewport Position
        Vector3 playerViewportPos = mainCamera.WorldToViewportPoint(playerWorldPos);
        Debug.Log($"Player Viewport Position: {playerViewportPos}");

        // 4. Canvas Position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.GetComponent<RectTransform>(), playerScreenPos, mainCamera, out playerCanvasPosition);
        Debug.Log($"Player Canvas Position: {playerCanvasPosition}");

        // 5. Local Position (relative to parent)
        Vector3 playerLocalPos = playerTransform.localPosition;
        Debug.Log($"Player Local Position: {playerLocalPos}");

        // 6. Anchored Position (if player has a RectTransform)
        RectTransform playerRectTransform = playerTransform.GetComponent<RectTransform>();
        if (playerRectTransform != null)
        {
            Vector2 playerAnchoredPos = playerRectTransform.anchoredPosition;
            Debug.Log($"Player Anchored Position: {playerAnchoredPos}");
        }
    }

    private void UpdateExpPosition()
    {
        RectTransform expRectTransform = experiencePrefab.rectTransform;

        // 1. World Position
        Vector3 worldPosition = expRectTransform.position;
        Debug.Log($"EXP World Position: {worldPosition}");

        // 2. Screen Position
        Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(mainCamera, worldPosition);
        Debug.Log($"EXP Screen Position: {screenPosition}");

        // 3. Viewport Position
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(worldPosition);
        Debug.Log($"EXP Viewport Position: {viewportPosition}");

        // 4. Canvas Position
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.GetComponent<RectTransform>(), screenPosition, mainCamera, out canvasPosition);
        Debug.Log($"EXP Canvas Position: {canvasPosition}");

        // 5. Local Position (relative to parent)
        Vector3 localPosition = expRectTransform.localPosition;
        Debug.Log($"EXP Local Position: {localPosition}");

        // 6. Anchored Position
        Vector2 anchoredPosition = expRectTransform.anchoredPosition;
        Debug.Log($"EXP Anchored Position: {anchoredPosition}");
    }

    public IEnumerator DropItemMove()
    {
        // 여기에서, 코인 골드 ui 포지션으로 이동 ! 부터 !!!!
        if (mainCanvas == null)
        {
            Debug.LogError("DropItem: Canvas not found. Cannot move items.");
            yield break;
        }
        
        // 코인 이동
        if (coinPrefab != null && goldUIPosition != null)
        {
            StartCoroutine(MoveCoinToUI(duration));
        }
        
        // 경험치, 포션 이동
        StartCoroutine(MoveToPlayer(duration));
        
        yield return new WaitForSeconds(duration);
        
        // 아이템 획득 완료 이벤트 발생
        OnItemCollected?.Invoke();
        
        yield return null;
    }
    
    // 포션, 경험치 : 플레이어로 이동
    private IEnumerator MoveToPlayer(float duration)
    {
        if (experiencePrefab == null || playerTransform == null)
        {
            Debug.LogError("MoveToPlayer: Experience prefab or Player transform is null.");
            yield break;
        }
        
        // EXP UI 요소의 현재 위치를 월드 좌표로 변환
        Vector3 expWorldStartPos = experiencePrefab.transform.position;
        expWorldStartPos = mainCamera.ScreenToWorldPoint(new Vector3(expWorldStartPos.x, expWorldStartPos.y, mainCamera.nearClipPlane));
        
        // Potion UI 요소의 현재 위치를 월드 좌표로 변환
        Vector3 potionWorldStartPos = potionPrefab.transform.position;
        potionWorldStartPos = mainCamera.ScreenToWorldPoint(new Vector3(potionWorldStartPos.x, potionWorldStartPos.y, mainCamera.nearClipPlane));
        
        // 플레이어의 월드 위치
        Vector3 playerWorldPos = playerTransform.position;

        Debug.Log($"EXP World Start Position: {expWorldStartPos}");
        Debug.Log($"Potion World Start Position: {potionWorldStartPos}");
        Debug.Log($"Player World Position: {playerWorldPos}");
        
        // 1) EXP 이동 : 3d 월드 공간(플레이어 위치)로 이동시키며, 위치를 2d scrren 좌표로 변환해서 ui를 이동 
        Tween moveTween = DOTween.To(() => expWorldStartPos, // 시작값(exp의 초기 월드 위치)
            pos =>  // pos : exp의 월드 위치 (보간된 값) 
            {
                Vector3 screenPos = mainCamera.WorldToScreenPoint(pos); // exp의 월드 좌표(pos)를 스크린 좌표(현재 화면상 위치)로 변환
                experiencePrefab.rectTransform.position = screenPos; // exp UI 위치를 계산된 스크린 좌표로 설정
            },
            playerWorldPos, duration); // 목표 월드 위치(플레이어), 지속시간

        // Potion 이동
        Tween potionMoveTween = DOTween.To(() => potionWorldStartPos,
            pos => 
            {
                Vector3 screenPos = mainCamera.WorldToScreenPoint(pos);
                potionPrefab.rectTransform.position = screenPos;
            },
            playerWorldPos, duration);
        
        yield return new WaitForSeconds(duration);

        Debug.Log($"EXP Final Screen Position: {experiencePrefab.rectTransform.position}");
        Debug.Log($"Potion Final Screen Position: {potionPrefab.rectTransform.position}");

        // 아이템 제거
        Destroy(experiencePrefab.gameObject);
        Destroy(potionPrefab.gameObject);
        Debug.Log("Experience item moved to player and destroyed.");
        Debug.Log("Potion item moved to player and destroyed.");

    }
    
    // 코인 : UI로 이동
    private IEnumerator MoveCoinToUI(float duration)
    {
        if (coinPrefab == null || goldUIPosition == null)
        {
            Debug.LogWarning("MoveCoinToUI: Coin prefab or Gold UI position is null.");
            yield break;
        }
        
        // 코인의 부모를 goldUIPosition으로 변경, 월드 포지션 유지 
        coinPrefab.transform.SetParent(goldUIPosition, true);
        
        // 목표 위치 : Gold UI의 중심
        Vector3 targetPosition = Vector3.zero;

        Debug.Log($"MoveCoinToUI - Start position: {coinPrefab.rectTransform.localPosition}, Target: {targetPosition}");

        // 코인 이동 애니메이션
        coinPrefab.rectTransform.DOLocalMove(targetPosition, duration).SetEase(Ease.InOutQuad);

        // 이동이 완료될 때까지 기다립니다.
        yield return new WaitForSeconds(duration);

        Debug.Log($"MoveCoinToUI - Final position: {coinPrefab.rectTransform.localPosition}, Parent: {coinPrefab.transform.parent.name}");
        
        // 코인 오브젝트 파괴
        Destroy(coinPrefab.gameObject);
        Debug.Log("Coin object destroyed.");
    }
}
