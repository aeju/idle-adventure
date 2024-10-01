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

    [SerializeField] private GameObject dropItemPrefab;
    
    // 지면으로 띄우기 
    void ItemDrop()
    {
        // "1. MainCanvas"인 캔버스 찾아서, 컴포넌트 가져오기 
        Canvas uiCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        
        if (uiCanvas == null)
        {
            Debug.LogError("Canvas with tag 'MainCanvas' not found");
            return;
        }
        Debug.Log($"Canvas found: {uiCanvas.name}");

        if (dropItemPrefab == null)
        {
            Debug.LogError("Drop Item Prefab is not assigned");
            return;
        }

        // 2. 내 위치를 캔버스 로컬 위치로 변환, DropItem 생성 
        Vector3 worldPosition = transform.position; // 1. 월드 포지션 
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition); // 2. 월드 포지션 -> 스크린 포지션
        Vector2 canvasPosition; // 3. 스크린 포지션 -> 캔버스 로컬 포지션 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvas.transform as RectTransform, screenPosition, uiCanvas.worldCamera, out canvasPosition);

        Debug.Log($"World Position: {worldPosition}, Screen Position: {screenPosition}, Canvas Position: {canvasPosition}");
        
        // dropItemPrefab 생성 (캔버스 자식으로) 
        GameObject droppedItem = Instantiate(dropItemPrefab, uiCanvas.transform);
        if (droppedItem == null)
        {
            Debug.LogError("Failed to instantiate dropItemPrefab");
            return;
        }
        Debug.Log($"Dropped item created: {droppedItem.name}");

        // 생성된 아이템의 위치, 캔버스 위치로 설정
        droppedItem.transform.localPosition = canvasPosition;
        Debug.Log($"Dropped item position set to: {droppedItem.transform.localPosition}");

        
        /*
        Vector3 dropPosition = transform.position + new Vector3(0, 1.0f, 0);
        GameObject droppedItem = Instantiate(dropItem, dropPosition, Quaternion.identity);
        
        DropItem dropItemScript = droppedItem.GetComponent<DropItem>();
        if (dropItemScript != null)
        {
           //dropItemScript.MoveToPlayer();
           dropItemScript.OnItemCollected += EarnRewards;  // 이벤트 구독
           // StartCoroutine(dropItemScript.MoveToPlayer());
           StartCoroutine(dropItemScript.DropItemMove());
        }
        else
        {
            Debug.LogError("DropItem script is not attached to the dropItem prefab");
        }
        */
    }

    void EarnRewards()
    {
        if (userInfo != null) // 경험치
        {
            userInfo.AddExperience(monsterStats.Exp);
        }

        if (resourceInfo != null) // 코인
        {
            resourceInfo.AddCoin(monsterStats.Coin);
        }
    }
}
