using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public partial class EnemyFSM : MonoBehaviour
{
    void CreateDamageText(int playerHitPower)
    {
        if (hudDamageText != null) // 데미지 텍스트 
        {
            // flipX을 기준으로 위치 계산
            float offsetDirection = flipX ? -1.0f : 1.0f;
            Vector3 damagePosition = transform.position + new Vector3(offsetDirection * 1.0f, 2.0f, 0);
            Utilities.CreateDamageText(hudDamageText, transform, playerHitPower, damagePosition, flipX);
        }
        else
        {
            Debug.LogError("HUD Damage Text prefab is not assigned");
        }
    }
    
    // 1. 아이템 생성 
    // 2. 아이템 이동 (ItemDrops에서)
    // 3. 아이템 획득 -> 리워드 반영
    void ItemDrop()
    {
        // "1. MainCanvas"인 캔버스 찾아서, 컴포넌트 가져오기 
        uiCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        if (uiCanvas == null)
        {
            Debug.LogError("Canvas with tag 'MainCanvas' not found");
        }
        
        // 2. 월드 좌표 - 캔버스 좌표로 변환
        Vector2 canvasPosition = CalculateCanvasPosition();
        // 3. dropItemPrefab 생성 (캔버스 자식으로) 
        GameObject droppedItem = CreateDroppedItem(canvasPosition);
        // 4. 아이템 획득 관련 이벤트 구독 
        if (droppedItem != null)
        {
            SubscribeToRewardEvents(droppedItem);
        }
    }

    private Vector2 CalculateCanvasPosition()
    {
        Vector3 worldPosition = transform.position; // 1. 월드 포지션 
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition); // 2. 월드 포지션 -> 스크린 포지션
        Vector2 canvasPosition; // 3. 스크린 포지션 -> 캔버스 로컬 포지션 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        uiCanvas.transform as RectTransform, 
            screenPosition, 
            uiCanvas.worldCamera, 
            out canvasPosition
        );
        return canvasPosition;
    }
    
    private GameObject CreateDroppedItem(Vector2 canvasPosition)
    {
        GameObject dropItem = Instantiate(dropItemPrefab, uiCanvas.transform);
        if (dropItem != null)
        {
            dropItem.transform.localPosition = canvasPosition;
            return dropItem;
        }
        else
        {
            Debug.LogError("Failed to instantiate dropItemPrefab");
            return null;
        }
    }

    private void SubscribeToRewardEvents(GameObject dropItem)
    {
        DropItemController dropItemController = dropItem.GetComponent<DropItemController>();
        if (dropItemController != null)
        {
            dropItemController.OnItemCollected += EarnRewards;  // 이벤트 구독
            dropItemController.DropItemMove();
        }
    }
    
    void EarnRewards()
    {
        SoundManager.Instance.PlaySFX("Earn");
        
        if (userInfo != null) // 경험치
        {
            userInfo.AddExperience(monsterStats.Exp);
        }

        if (resourceInfo != null) // 코인
        {
            resourceInfo.AddCoin(monsterStats.Coin);
        }

        if (potionInfo != null)
        {
            // 랜덤 : 이상 ~ 미만 
            int randomPotionAmount = UnityEngine.Random.Range(monsterStats.MinPotionDrop, monsterStats.MaxPotionDrop + 1);
            potionInfo.AddPotion(randomPotionAmount);
        }
    }
}
