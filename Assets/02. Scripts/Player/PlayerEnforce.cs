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

    // 업그레이드 실행을 위한 메서드
    public void Upgrade(Action upgradeAction)
    {
        if (ResourceManager.Instance.current_Coin >= cost)
        {
            upgradeAction(); // 업그레이드
            ResourceManager.Instance.current_Coin -= cost;
            
            totalIncrease += increaseAmount;
            cost++; 
            level++;

            // UI 업데이트
            costText.text = NumberFormatter.FormatNumberUnit(cost);
            levelText.text = "Lv. " + level.ToString();
            totalIncreaseText.text = "+" + totalIncrease;
        }
        else // 보유 코인 부족
        {
            Debug.Log("Not enough coins");
        }
    }
}

public class PlayerEnforce : EnforceSubject
{
    //public UpgradeOption attackUpgrade, maxHPUpgrade, defenseUpgrade;
    private UpgradeOption attackUpgrade, maxHPUpgrade, defenseUpgrade;
    
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
            //alertPopup.SetActive(false);
            alertPopup.SetActive(true);
        }
        
        // new
        attackUpgrade = new UpgradeOption
        {
            upgradeButton = attackBtn,
            increaseAmount = attackIncrease,
            cost = attackCost,
            costText = attackCostText,
            levelText = attackLevelText,
            totalIncreaseText = totalAttackIncreaseText
        };
        
        maxHPUpgrade = new UpgradeOption
        {
            upgradeButton = maxHPBtn,
            increaseAmount = maxHPIncrease,
            cost = maxHPCost,
            costText = maxHPCostText,
            levelText = maxHPLevelText,
            totalIncreaseText = totalMaxHPIncreaseText
        };
        
        defenseUpgrade = new UpgradeOption
        {
            upgradeButton = defenseBtn,
            increaseAmount = defenseIncrease,
            cost = defenseCost,
            costText = defenseCostText,
            levelText = defenseLevelText,
            totalIncreaseText = totalDefenseIncreaseText
        };
    }

    void Start()
    {
        attackUpgrade.upgradeButton.OnClickAsObservable().Subscribe(_ =>
        {
            attackUpgrade.Upgrade(() =>
            {
                playerStats.attack += attackUpgrade.increaseAmount;
                totalAttackIncrease += attackUpgrade.increaseAmount;
            });
            NotifyObservers();
        }).AddTo(this);
        
        maxHPUpgrade.upgradeButton.OnClickAsObservable().Subscribe(_ =>
        {
            maxHPUpgrade.Upgrade(() =>
            {
                playerStats.maxHP += maxHPUpgrade.increaseAmount;
                totalMaxHPIncrease += maxHPUpgrade.increaseAmount;
            });
            NotifyObservers();
        }).AddTo(this);
        
        defenseUpgrade.upgradeButton.OnClickAsObservable().Subscribe(_ =>
        {
            defenseUpgrade.Upgrade(() =>
            {
                playerStats.defense += defenseUpgrade.increaseAmount;
                totalDefenseIncrease += defenseUpgrade.increaseAmount;
            });
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
    
    
    // 버튼 (공격력/HP/방어력)
    //[SerializeField] private Button attackBtn, maxHPBtn, defenseBtn;
    [SerializeField] private Button attackBtn, maxHPBtn, defenseBtn;
    
    // 업그레이드 증가량
    public int attackIncrease = 2;
    public int maxHPIncrease = 15;
    public int defenseIncrease = 1;
    
    // 총 증가량  
    public int totalAttackIncrease = 0;
    public int totalMaxHPIncrease = 0;
    public int totalDefenseIncrease = 0 ;

    public TextMeshProUGUI totalAttackIncreaseText;
    public TextMeshProUGUI totalMaxHPIncreaseText;
    public TextMeshProUGUI totalDefenseIncreaseText;

    // 골드 비용
    public int attackCost = 100;
    public int maxHPCost = 100;
    public int defenseCost = 100;

    public TextMeshProUGUI attackCostText;
    public TextMeshProUGUI maxHPCostText;
    public TextMeshProUGUI defenseCostText;
    
    // 레벨
    //public int attackLevel = 0;
    //public int maxHPLevel = 0;
    //public int defenseLevel = 0;

    public TextMeshProUGUI attackLevelText;
    public TextMeshProUGUI maxHPLevelText;
    public TextMeshProUGUI defenseLevelText;
    
    /*
    private ResourceManager resourceInfo;
    
    // 옵저버 패턴
    private PlayerStats playerStats;
    
    private ResourceBar resourceBar;
    
    // 경고 팝업
    [SerializeField] private GameObject alertPopup;

    void Awake()
    {
        resourceInfo = ResourceManager.Instance;
        playerStats = FindObjectOfType<PlayerStats>();
        resourceBar = FindObjectOfType<ResourceBar>();

        if (alertPopup != null)
        {
            alertPopup.SetActive(false);
        }
    }
    
    void Start()
    {
        UpdateCostUI();
        UpdateStatsUI();
        UpdateLevelUI();
        
        attackBtn.OnClickAsObservable().Subscribe(_ =>
        {
            UpgradeAttack();
            NotifyObservers();
        }).AddTo(this);

        maxHPBtn.OnClickAsObservable().Subscribe(_ =>
        {
            UpgradeHP();
            NotifyObservers();
        }).AddTo(this);
        
        defenseBtn.OnClickAsObservable().Subscribe(_ =>
        {
            UpgradeDefense();
            NotifyObservers();
        }).AddTo(this);
    }

    // 스탯 업그레이드 메소드 일반화
    
    
    void UpdateCostUI()
    {
        attackCostText.text = NumberFormatter.FormatNumberUnit(attackCost);
        maxHPCostText.text = NumberFormatter.FormatNumberUnit(maxHPCost);
        defenseCostText.text = NumberFormatter.FormatNumberUnit(defenseCost);
    }

    void UpdateLevelUI()
    {
        attackLevelText.text = "Lv. " + attackLevel.ToString();
        maxHPLevelText.text = "Lv. " + maxHPLevel.ToString();
        defenseLevelText.text = "Lv. " + defenseLevel.ToString();
    }
    
    void UpdateStatsUI()
    {
        totalAttackIncreaseText.text = "+" + totalAttackIncrease;
        totalHpIncreaseText.text = "+" + totalHpIncrease;
        totalDefenseIncreaseText.text = "+" + totalDefenseIncrease;
    }

    private void UpgradeAttack()
    {
        if (resourceInfo.current_Coin >= attackCost)
        {
            playerStats.attack += attackIncrease;
            resourceInfo.current_Coin -= attackCost;
            
            totalAttackIncrease += attackIncrease; // 스탯 증가
            attackCost++; // 강화 비용 증가
            attackLevel++; // 강화 레벨 증가
            
            UpdateCostUI();
            UpdateStatsUI();
            UpdateLevelUI();
        }
        else // 경고 
        {
            // 이렇게가 아니고, 버튼 클릭 -> UI / 전투력 반영, 보유 코인 반영, 코인 버튼 색 업데이트
            Debug.Log("Coin 부족");
            attackCostText.color = Color.red; // 이 때말고, 버튼 클릭 후 판단하는 걸로 변경 
            // UI팝업 생성
            if (alertPopup != null && !alertPopup.activeSelf)
            {
                alertPopup.SetActive(true);
            }
        }
    }

    // 강화 + current HP 초기화 
    private void UpgradeHP()
    {
        Debug.Log("1. playerHP : " + playerStats.maxHP);
        if (resourceInfo.current_Coin >= maxHPCost)
        {
            Debug.Log("HP Upgrade");
            playerStats.maxHP += hpIncrease;
            playerStats.currentHP = playerStats.maxHP;
            // 필요: PlayerController HPSliderUpdate();
            Debug.Log("2. playerHP : " + playerStats.maxHP);
            resourceInfo.current_Coin -= maxHPCost;
            
            totalHpIncrease += hpIncrease;
            maxHPCost++;
            maxHPLevel++;
            
            UpdateCostUI();
            UpdateStatsUI();
            UpdateLevelUI();
        }
        else
        {
            Debug.Log("Coin 부족");
        }
    }
    
    // 데미지 식에 반영 
    private void UpgradeDefense()
    {
        Debug.Log("1. playerDefense : " + playerStats.maxHP);
        if (resourceInfo.current_Coin >= defenseCost)
        {
            Debug.Log("Defense Upgrade");
            playerStats.defense += defenseIncrease;
            Debug.Log("2. playerDefense : " + playerStats.defense);
            resourceInfo.current_Coin -= defenseCost;
            
            totalDefenseIncrease += defenseIncrease;
            defenseCost++;
            defenseLevel++;
            
            UpdateCostUI();
            UpdateStatsUI();
            UpdateLevelUI();
        }
        else
        {
            Debug.Log("Coin 부족");
        }
    }

    
    */
}
