using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DropItemController : MonoBehaviour
{   
    // 드랍 아이템 - 이미지 (코인, exp, 포션) 
    [SerializeField] private Image coinPrefab;
    [SerializeField] private Image experiencePrefab;
    [SerializeField] private Image potionPrefab;
    
    // 목적지 (플레이어, goldUI, expUI)
    private Transform playerTransform;
    private RectTransform coinUIPosition;
    private RectTransform expUIPosition;
    
    // 아이템 획득 완료 이벤트
    public delegate void ItemCollectedHandler();
    public event ItemCollectedHandler OnItemCollected;
    
    private Camera mainCamera;
    private Canvas mainCanvas;
    
    private Vector2 playerScreenPosition;
    private Vector2 playerCanvasPosition;

    private int activeItemCount = 0;
    [SerializeField] private float duration = 0.6f;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        // 플레이어 
        playerTransform = GameObject.FindGameObjectWithTag("player")?.transform;
        LogComponentStatus("Player", playerTransform);
        
        // 코인
        coinUIPosition = GameObject.FindGameObjectWithTag("GoldUI")?.GetComponent<RectTransform>();
        LogComponentStatus("GoldUI", coinUIPosition);
        
        // exp 
        expUIPosition = GameObject.FindGameObjectWithTag("ExpUI")?.GetComponent<RectTransform>();
        LogComponentStatus("ExpUI", expUIPosition);
        
        mainCamera = Camera.main;
        LogComponentStatus("Main Camera", mainCamera);
        
        mainCanvas = GetComponentInParent<Canvas>(); // mainCanvas : 부모 오브젝트 중 Canvas 컴포넌트
        LogComponentStatus("Canvas", mainCanvas);
    }
    
    private void LogComponentStatus(string componentName, Object component)
    {
        if (component == null)
        {
            Debug.LogWarning($"{componentName} not found!");
        }
    }
    
    private void OnEnable()
    {
        UpdatePlayerPosition(); // 플레이어 위치 
    }

    private void UpdatePlayerPosition()
    {
        if (playerTransform == null || mainCamera == null || mainCanvas == null) return;
        
        // 1. World Position
        Vector3 playerWorldPos = playerTransform.position;
        // 2. Screen Position
        Vector2 playerScreenPos = mainCamera.WorldToScreenPoint(playerWorldPos);
        
        // 3. Canvas Position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            mainCanvas.GetComponent<RectTransform>(), 
            playerScreenPos, 
            mainCamera, 
            out playerCanvasPosition);
    }
    
    public void DropItemMove()
    {
        if (mainCanvas == null)
        {
            Debug.LogError("DropItem: Canvas not found. Cannot move items.");
            return;
        }
        
        activeItemCount = 0;
        
        // 1. 코인 이동
        if (coinPrefab != null && coinUIPosition != null)
        {
            activeItemCount++;
            MoveCoinToUI();
        }
            
    
        // 2. 경험치 이동
        if (experiencePrefab != null && expUIPosition != null)
        {
            activeItemCount++;
            MoveExpToUI();
        }
        
        // 3. 포션 이동
        if (potionPrefab != null && playerTransform != null)
        {
            activeItemCount++;
            MoveToPlayer();
        }
        
        // 모든 아이템 이동이 완료 -> 완료 이벤트 발생, 오브젝트 파괴
        if (activeItemCount == 0)
        {
            OnAllItemsMoved();
        }
        
        /*
        yield return new WaitForSeconds(duration);
        
        // 아이템 획득 완료 이벤트 발생
        OnItemCollected?.Invoke();
        
        Destroy(gameObject);
        */
    }
    
    private void OnItemMoved()
    {
        activeItemCount--;
        if (activeItemCount == 0)
        {
            OnAllItemsMoved();
        }
    }
    
    private void OnAllItemsMoved()
    {
        // 아이템 획득 완료 이벤트 발생
        OnItemCollected?.Invoke();
        Destroy(gameObject);
    }
    
    // 1. 코인 : 코인 이미지 UI로 이동
    private void MoveCoinToUI()
    {
        // 코인의 부모를 goldUIPosition으로 변경, 월드 포지션 유지 
        coinPrefab.transform.SetParent(coinUIPosition, true);
        
        // 목표 위치 : Gold UI의 중심
        Vector3 targetPosition = Vector3.zero;
        
        coinPrefab.rectTransform.DOLocalMove(targetPosition, duration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => 
            {
                Destroy(coinPrefab.gameObject);
                OnItemMoved();
            });
            //.OnComplete(() => Destroy(coinPrefab.gameObject)); // 아이템 제거
    }
    
    private void MoveExpToUI()
    {
        experiencePrefab.transform.SetParent(expUIPosition, true);
        
        // 목표 위치 : 경험치 바 내 x좌표 랜덤 (y축은 0고정) 
        Vector2 expBarSize = expUIPosition.rect.size; // 경험치 바의 크기를 가져옴
        Vector2 targetPosition = new Vector2(Random.Range(0, expBarSize.x), 0); 
        
        // 1. 목표 위치로 이동 (duration의 2/3 사용)
        // 2. 크기를 1/3으로 줄임 (duration의 1/3 사용)
        Sequence sequence = DOTween.Sequence();
        sequence.Append(experiencePrefab.rectTransform.DOLocalMove(targetPosition, duration * 0.66f).SetEase(Ease.InOutQuad))
                .Append(experiencePrefab.transform.DOScale(Vector3.one * 0.3f, duration * 0.33f).SetEase(Ease.InOutQuad))
                .OnComplete(() => 
                {
                    Destroy(experiencePrefab.gameObject);
                    OnItemMoved();
                });
                //.OnComplete(() => Destroy(experiencePrefab.gameObject));
    }
    
    // 3. 포션 : 플레이어로 이동
    private void MoveToPlayer()
    {
        // Potion UI 요소의 현재 위치를 월드 좌표로 변환
        Vector3 potionWorldStartPos = potionPrefab.transform.position;
        potionWorldStartPos = mainCamera.ScreenToWorldPoint(new Vector3(potionWorldStartPos.x, potionWorldStartPos.y, mainCamera.nearClipPlane));
        
        // 목표 위치 : 플레이어의 월드 위치 (y축 + 0.5f)
        Vector3 playerWorldPos = playerTransform.position + new Vector3(0, 0.5f, 0);
        
        DOTween.To(() => potionWorldStartPos,
                pos => 
                {
                    Vector3 screenPos = mainCamera.WorldToScreenPoint(pos);
                    potionPrefab.rectTransform.position = screenPos;
                },
                playerWorldPos, duration)
            .OnComplete(() => 
            {
                Destroy(potionPrefab.gameObject);
                OnItemMoved();
            });
            //.OnComplete(() => Destroy(potionPrefab.gameObject)); 
    }
}
