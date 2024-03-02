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
    //public int level = 0;
    private int level = 0;
    //public int totalIncrease = 0;
    private int totalIncrease = 0;

    private Action onNotEnoughCoins; // 코인 부족 시 실행할 콜백
    
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
            onNotEnoughCoins?.Invoke(); // 코인 부족 콜백 실행
        }
    }
    
    public void UIUpdate()
    {
        costText.text = NumberFormatter.FormatNumberUnit(cost);
        levelText.text = "Lv. " + level.ToString();
        totalIncreaseText.text = "+" + totalIncrease;
    }
    
    // 코인 부족 -> 실행할 콜백
    public void SetOnNotEnoughCoinsAction(Action action)
    {
        onNotEnoughCoins = action;
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
        playerStats = FindObjectOfType<PlayerStats>();
        resourceBar = FindObjectOfType<ResourceBar>();

        alertPopup?.SetActive(false);
    }

    void Start()
    {
        InitUpgradeOptions(attackUpgrade, amount => playerStats.attack += amount);
        InitUpgradeOptions(maxHPUpgrade, amount => playerStats.maxHP += amount);
        InitUpgradeOptions(defenseUpgrade, amount => playerStats.defense += amount);
    }
    
    // UI 초기화, 코인 부족 콜백, 스탯 증가 이벤트 구독
    void InitUpgradeOptions(UpgradeOption upgradeOption, Action<int> statIncreaseAction)
    {
        // 초기 UI 상태 설정
        upgradeOption.UIUpdate();
    
        // 코인 부족, 콜백 설정
        upgradeOption.SetOnNotEnoughCoinsAction(ShowAlertPopup);
    
        // 스탯 증가 이벤트 구독
        upgradeOption.upgradeButton.OnClickAsObservable().Subscribe(_ =>
        {
            upgradeOption.Upgrade(() => statIncreaseAction(upgradeOption.increaseAmount));
            NotifyObservers();
        }).AddTo(this);
    }

    // 경고 팝업 활성화
    void ShowAlertPopup()
    {
        if (alertPopup != null)
        {
            alertPopup.SetActive(true);
        }
    }
    
    // 옵저버 연결 활성화, 비활성화
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
