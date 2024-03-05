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
    [SerializeField] private int cost = 100;
    [SerializeField] private TextMeshProUGUI costText, levelText, totalIncreaseText;
    [SerializeField] private int level = 0;
    [SerializeField] private int maxLevel;
    [SerializeField] private int totalIncrease = 0;
    [SerializeField] private Slider levelSlider;
    [SerializeField] private Image upgradeAvailableImage;
    [SerializeField] private Image upgradeUnavailableImage;
    [SerializeField] private TextMeshProUGUI buttonText;

    private Action onNotEnoughCoins; // 코인 부족 시 실행할 콜백

    // 업그레이드 가능 : 현재 레벨 < 최대 레벨, 강화 비용 <= 보유 코인
    private bool CanUpgrade()
    {
        return level < maxLevel && ResourceManager.Instance.current_Coin >= cost;
    }
    
    // 스탯 강화
    public void Upgrade(Action upgradeAction)
    {
        if (CanUpgrade()) // 업그레이드 가능
        {
            upgradeAction(); // 업그레이드
            ResourceManager.Instance.UseCoin(cost); // 코인 사용
            
            totalIncrease += increaseAmount; // 증가량 
            cost++; // 비용
            level++; // 레벨

            // UI 업데이트
            UIUpdate();
            CheckUpgrade(); 
        }
        else // 업그레이드 불가능
        {
            onNotEnoughCoins?.Invoke(); // 코인 부족 콜백 실행
        }
    }

    // 업그레이드 가능 상태 검사 + UI 초기화
    public void UIInit()
    {
        UIUpdate();
        CheckUpgrade(); 
    }
    
    public void UIUpdate()
    {
        costText.text = Utilities.FormatNumberUnit(cost);
        levelText.text = "Lv. " + level.ToString();
        totalIncreaseText.text = "+" + totalIncrease;
        buttonText.text = "레벨업";
        
        if (levelSlider != null)
        {
            levelSlider.value = (float)level / maxLevel; 
        }
    }
    
    public void CheckUpgrade()
    {
        bool isMaxLevelReached = level >= maxLevel;
        bool hasNotEnoughCoins = ResourceManager.Instance.current_Coin < cost;
        
        // 강화 완료
        if (isMaxLevelReached)
        {
            upgradeButton.interactable = false; // 버튼 비활성화
            upgradeAvailableImage.gameObject.SetActive(false);
            upgradeUnavailableImage.gameObject.SetActive(false);
            buttonText.text = "강화 완료";
            costText.text = "";
            costText.color = Color.black; 
            buttonText.color = Color.white; 
        }
        // 코인 부족
        else if (hasNotEnoughCoins)
        {
            upgradeAvailableImage.gameObject.SetActive(false);
            upgradeUnavailableImage.gameObject.SetActive(true);
            costText.color = Color.red; 
            buttonText.color = Color.black; 
        }
        else // 강화 가능
        {
            upgradeAvailableImage.gameObject.SetActive(true);
            upgradeUnavailableImage.gameObject.SetActive(false);
            costText.color = Color.white; 
            buttonText.color = Color.white; 
        }
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
        
        // ResourceManager의 코인 변경을 구독하여 모든 UpgradeOption의 UI 업데이트
        ResourceManager.Instance.OnResourcesUpdated += UpdateAllUpgradeOptionsUI;
    }
    
    // UI 초기화, 코인 부족 콜백, 스탯 증가 이벤트 구독
    void InitUpgradeOptions(UpgradeOption upgradeOption, Action<int> statIncreaseAction)
    {
        // 초기 UI 상태 설정
        upgradeOption.UIInit();
    
        // 코인 부족, 콜백 설정
        upgradeOption.SetOnNotEnoughCoinsAction(ShowAlertPopup);
    
        // 스탯 증가 이벤트 구독
        upgradeOption.upgradeButton.OnClickAsObservable().Subscribe(_ =>
        {
            upgradeOption.Upgrade(() => statIncreaseAction(upgradeOption.increaseAmount));
            NotifyObservers();
        }).AddTo(this);
    }
    
    // 모든 UpgradeOption의 UI 업데이트
    void UpdateAllUpgradeOptionsUI()
    {
        attackUpgrade.CheckUpgrade();
        maxHPUpgrade.CheckUpgrade();
        defenseUpgrade.CheckUpgrade();
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
            Attach(resourceBar);
        }
    }

    void OnDisable()
    {
        if (resourceBar)
            Detach(resourceBar);
    }
}