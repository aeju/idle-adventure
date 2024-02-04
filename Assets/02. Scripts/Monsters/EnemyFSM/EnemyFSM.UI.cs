using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public partial class EnemyFSM : MonoBehaviour
{
    void CreateDamageText(int hitPower)
    {
        if (hudDamageText != null) // 데미지 텍스트 
        {
            // flipX을 기준으로 위치 계산
            float offsetDirection = flipX ? -1.0f : 1.0f;
            Vector3 damagePosition = transform.position + new Vector3(offsetDirection * 1.0f, 2.0f, 0);
            GameObject damageText = Instantiate(hudDamageText, damagePosition, Quaternion.identity, transform); // 자식으로 생성
            damageText.GetComponent<DamageText>().damage = hitPower;
        }
    }
    
    // 현재 몬스터 hp(%)를 hp 슬라이더의 value에 반영
    void HPSliderUpdate()
    {
        //hpSlider.value = Mathf.Lerp((float) hpSlider.value, (float)currentHP / (float)maxHP, Time.deltaTime * 100);
        hpSlider.value = (float) monsterStats.currentHP / (float) monsterStats.maxHP; 
    }
    
    // 지면으로 띄우기 
    void ItemDrop()
    {
        Vector3 dropPosition = transform.position + new Vector3(0, 1.0f, 0);
        GameObject droppedItem = Instantiate(dropItem, dropPosition, Quaternion.identity);

        StartCoroutine(MoveItemToPlayer(droppedItem));
    }
    
    IEnumerator MoveItemToPlayer(GameObject item)
    {
        float duration = 1.0f; // 이동 
        Vector3 playerPosition = target.transform.position; 
        
        Tween moveTween = item.transform.DOMove(playerPosition, duration).SetEase(Ease.InOutQuad);
        yield return moveTween.WaitForCompletion();
        Destroy(item);
        EarnRewards();
    }

    void EarnRewards()
    {
        if (userInfo != null)
        {
            userInfo.AddExperience(monsterStats.exp);
        }

        if (resourceInfo != null)
        {
            resourceInfo.AddCoin(monsterStats.coin);
        }
    }
}
