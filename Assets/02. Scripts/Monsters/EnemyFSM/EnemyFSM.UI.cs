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

    // 지면으로 띄우기 
    void ItemDrop()
    {
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
