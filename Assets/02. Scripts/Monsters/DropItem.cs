using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropItem : MonoBehaviour
{
    public GameObject coinPrefab;
    public GameObject experiencePrefab;
    public GameObject potionPrefab;

    private Transform playerTransform;
    private RectTransform goldUIPosition;
    
    // 아이템 획득 완료 이벤트
    public delegate void ItemCollectedHandler();
    public event ItemCollectedHandler OnItemCollected;
    
    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("player")?.transform;
        if (!playerTransform)
        {
            Debug.LogWarning("DropItem: Player not found!");
        }
        
        GameObject goldUIObject = GameObject.FindGameObjectWithTag("GoldUI");
        if (goldUIObject != null)
        {
            goldUIPosition = goldUIObject.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogWarning("DropItem: GoldUI not found!");
        }
    }
    
    // public void MoveToPlayer()
    // public IEnumerator MoveToPlayer()
    public IEnumerator DropItemMove()
    {
        if (playerTransform == null)
        {
            Debug.LogError("DropItem: Cannot move to player. Player transform is null.");
            yield break;
        }

        float duration = 1.0f;
        Vector3 playerPosition = playerTransform.position;
        
        // DropItem을 플레이어에게 이동
        Tween moveTween = transform.DOMove(playerPosition, duration).SetEase(Ease.InOutQuad);
        yield return moveTween.WaitForCompletion();
        
        // 아이템 획득 완료 이벤트 발생
        OnItemCollected?.Invoke();
        
        Destroy(gameObject);
    }

    // 포션, 경험치 : 플레이어로 이동
    public void MoveToPlayer()
    {
        
    }
    
    // 코인 : UI로 이동
    public void MoveToUI()
    {
        
    }
}
