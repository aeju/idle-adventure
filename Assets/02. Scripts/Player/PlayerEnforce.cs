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
    [SerializeField] private Button attackBtn;
    [SerializeField] private Button maxHPBtn;
    [SerializeField] private Button defenseBtn;
    
    // 비용
    private float upgradeCoinCost;
    
    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        resourceInfo = ResourceManager.Instance;

        attackBtn = GetComponent<Button>();
        attackBtn.OnClickAsObservable().Subscribe(_ =>
        {
            UpgradeAttack();
        }).AddTo(this);
        
        maxHPBtn = GetComponent<Button>();
        maxHPBtn.OnClickAsObservable().Subscribe(_ =>
        {
            UpgradeHP();
        }).AddTo(this);

        defenseBtn = GetComponent<Button>();
        defenseBtn.OnClickAsObservable().Subscribe(_ =>
        {
            UpgradeDefense();
        }).AddTo(this);
    }

    private void UpgradeAttack()
    {
        if (resourceInfo.current_Coin >= upgradeCoinCost)
        {
            playerStats.attack += 5;
            resourceInfo.current_Coin -= upgradeCoinCost;
            UpdateGoldDisplay();
        }
        else // 경고
    }
    
    private void UpgradeHP()
    {
        
    }
    
    private void UpgradeDefense()
    {
        
    }

    private void UpdateGoldDisplay()
    {
        
    }
}
