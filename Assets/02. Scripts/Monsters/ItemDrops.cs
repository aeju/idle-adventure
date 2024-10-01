using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemDrops : MonoBehaviour
{
    //public GameObject coinPrefab;
    public Image coinPrefab;
    //public GameObject experiencePrefab;
    public Image experiencePrefab;
    //public GameObject potionPrefab;
    public Image potionPrefab;
    
    private Transform playerTransform;
    private RectTransform goldUIPosition;
    
    // 아이템 획득 완료 이벤트
    public delegate void ItemCollectedHandler();
    public event ItemCollectedHandler OnItemCollected;
    
    private Camera mainCamera;
    private Canvas mainCanvas;
    

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

    public IEnumerator DropItemMove()
    {
        // 여기에서, 코인 골드 ui 포지션으로 이동 ! 부터 !!!!
        if (mainCanvas == null)
        {
            Debug.LogError("DropItem: Canvas not found. Cannot move items.");
            yield break;
        }
        
        float duration = 2.0f;
        
        // 코인 이동
        if (coinPrefab != null && goldUIPosition != null)
        {
            yield return StartCoroutine(MoveCoinToUI(duration));
        }
        
        yield return new WaitForSeconds(duration);
        
        // 아이템 획득 완료 이벤트 발생
        OnItemCollected?.Invoke();
        
        /*
        
        //MoveToPlayer(playerPosition, duration);
        //MoveToPlayer(duration);
        
        yield return StartCoroutine(MoveToPlayer(duration));
        
        //MoveCoinToUI(duration);
        //MoveCoinToUI(5f);
        
        
        // 모든 이동이 완료된 후 아이템과 이 스크립트를 제거
        Destroy(gameObject);
        */
        
        yield return null;
    }
    
    // 포션, 경험치 : 플레이어로 이동
    //private void MoveToPlayer(Vector3 playerPosition, float duration)
    // private void MoveToPlayer(float duration)
    private IEnumerator MoveToPlayer(float duration)
    {
        if (playerTransform == null)
        {
            Debug.LogError("DropItem: Cannot move to player. Player transform is null.");
            yield break;
        }
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
