using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using System;

[Serializable]
public class UpgradeOption
{
    public Button upgradeButton;
    public int increaseAmount;
    public int cost = 100;
    public TextMeshProUGUI costText, levelText, totalIncreaseText;
    public int level = 0;
    public int totalIncrease = 0;

    // 스탯 강화
    public void Upgrade(Action upgradeAction)
    {
        if (ResourceManager.Instance.current_Coin >= cost)
        {
            upgradeAction(); // 업그레이드
            ResourceManager.Instance.current_Coin -= cost; // 비용 차감
            
            totalIncrease += increaseAmount; // 증가량 
            cost++; // 비용
            level++; // 레벨

            // UI 업데이트
            UIUpdate();
        }
        else // 보유 코인 부족
        {
            Debug.Log("Not enough coins");
        }
    }
    
    // 초기 UI 설정
    public void InitializeUI()
    {
        UIUpdate(); 
    }

    private void UIUpdate()
    {
        costText.text = NumberFormatter.FormatNumberUnit(cost);
        levelText.text = "Lv. " + level.ToString();
        totalIncreaseText.text = "+" + totalIncrease;
    }
}

public class PlayerEnforce : EnforceSubject
{
    public UpgradeOption attackUpgrade, maxHPUpgrade, defenseUpgrade;

    private PlayerStats playerStats;
    
    // 옵저버 패턴
    private ResourceBar resourceBar;
    
    // 경고 팝업
    [SerializeField] private GameObject alertPopup;
    
    void Awake()
    {
        // 기존
        playerStats = FindObjectOfType<PlayerStats>();
        resourceBar = FindObjectOfType<ResourceBar>();

        if (alertPopup != null)
        {
            alertPopup.SetActive(false);
        }
    }

    void Start()
    {
        // 초기 UI 상태 설정
        attackUpgrade.InitializeUI();
        maxHPUpgrade.InitializeUI();
        defenseUpgrade.InitializeUI();
        
        // 각 스탯 UpgradeOption에 대해 이벤트 구독
        SubscribeUpgradeEvent(attackUpgrade, () => playerStats.attack += attackUpgrade.increaseAmount);
        SubscribeUpgradeEvent(maxHPUpgrade, () => playerStats.maxHP += maxHPUpgrade.increaseAmount);
        SubscribeUpgradeEvent(defenseUpgrade, () => playerStats.defense += defenseUpgrade.increaseAmount);
    }

    private void SubscribeUpgradeEvent(UpgradeOption upgradeOption, Action action)
    {
        upgradeOption.upgradeButton.OnClickAsObservable().Subscribe(_ =>
        {
            upgradeOption.Upgrade(action);
            NotifyObservers();
        }).AddTo(this);
    }
    
    // 관찰자 연결 활성화, 비활성화
    void OnEnable()
    {
        if (resourceBar)
        {
            Debug.Log("Attach1");
            Attach(resourceBar);
        }
    }

    void OnDisable()
    {
        if (resourceBar)
            Detach(resourceBar);
    }
}
