using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Unity.VisualScripting;

// 1. 골드 차감
// 2. 스탯 올리기 (골드가 있을 때만) 
// 2-1. 골드가 없으면, 버튼 클릭x? 클릭 o + 경고문?
// 2-2. 골드 UI 업데이트 
public class PlayerEnforce : MonoBehaviour
{
    private PlayerStats playerStats;
    private ResourceManager resourceInfo;
    
    // 버튼
    //[SerializeField] private Button attackBtn;
    //[SerializeField] private Button maxHPBtn;
    //[SerializeField] private Button defenseBtn;
    
    public Button attackBtn;
    public Button maxHPBtn;
    public Button defenseBtn;
    
    // 비용
    private int upgradeCoinCost;

    // 증가량, 변수 이름 수정 필요 
    public int attack;
    public int hp;
    public int defense;
    
    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        resourceInfo = ResourceManager.Instance;

        
        attackBtn.OnClickAsObservable().Subscribe(_ =>
        {
            UpgradeAttack();
        }).AddTo(this);

        maxHPBtn.OnClickAsObservable().Subscribe(_ =>
        {
            UpgradeHP();
        }).AddTo(this);
        
        defenseBtn.OnClickAsObservable().Subscribe(_ =>
        {
            UpgradeDefense();
        }).AddTo(this);
    }

    // + 5 <- 변수로 변경 
    private void UpgradeAttack()
    {
        Debug.Log("1. playerAttack : " + playerStats.attack);
        if (resourceInfo.current_Coin >= upgradeCoinCost)
        {
            Debug.Log("Attack Upgrade");
            playerStats.attack += 5;
            Debug.Log("2. playerAttack : " + playerStats.attack);
            resourceInfo.current_Coin -= upgradeCoinCost;
            UpdateGoldDisplay();
        }
        else // 경고 팝업
        {
            Debug.Log("Coin 부족");
        }
    }
    
    // 강화 + current HP 초기화 
    private void UpgradeHP()
    {
        Debug.Log("1. playerHP : " + playerStats.maxHP);
        if (resourceInfo.current_Coin >= upgradeCoinCost)
        {
            Debug.Log("HP Upgrade");
            playerStats.maxHP += 5;
            playerStats.currentHP = playerStats.maxHP;
            // 필요: PlayerController HPSliderUpdate();
            Debug.Log("2. playerHP : " + playerStats.maxHP);
            resourceInfo.current_Coin -= upgradeCoinCost;
            UpdateGoldDisplay();
            UpdateStatsDisplay();
        }
    }
    
    // 데미지 식에 반영 
    private void UpgradeDefense()
    {
        Debug.Log("1. playerDefense : " + playerStats.maxHP);
        if (resourceInfo.current_Coin >= upgradeCoinCost)
        {
            Debug.Log("Defense Upgrade");
            playerStats.defense += 5;
            Debug.Log("2. playerDefense : " + playerStats.defense);
            resourceInfo.current_Coin -= upgradeCoinCost;
            UpdateGoldDisplay();
            UpdateStatsDisplay();
        }
    }

    // 리소스바 - 보유 골드 
    private void UpdateGoldDisplay()
    {
        
    }
    
    // 영웅 페이지 - 스탯 
    private void UpdateStatsDisplay()
    {
        
    }
}
