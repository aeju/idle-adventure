using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

// 1. 골드 차감
// 2. 스탯 올리기 (골드가 있을 때만) 
// 2-1. 골드가 없으면, 버튼 클릭x? 클릭 o + 경고문?
// 2-2. 골드 UI 업데이트 

// 코인 표시 = CoinFormatter.FormatCoinUnit(coin);
public class PlayerEnforce : EnforceSubject
{
    // 버튼
    [SerializeField] private Button attackBtn;
    [SerializeField] private Button maxHPBtn;
    [SerializeField] private Button defenseBtn;
    
    // 업그레이드 증가량
    public int attackIncrease = 2;
    public int hpIncrease = 15;
    public int defenseIncrease = 1;
    
    // 총 증가량  
    public int totalAttackIncrease = 0;
    public int totalHpIncrease = 0;
    public int totalDefenseIncrease = 0 ;

    public TextMeshProUGUI totalAttackIncreaseText;
    public TextMeshProUGUI totalHpIncreaseText;
    public TextMeshProUGUI totalDefenseIncreaseText;

    // 골드 비용
    public int attackCost = 100;
    public int maxHPCost = 100;
    public int defenseCost = 100;

    public TextMeshProUGUI attackCostText;
    public TextMeshProUGUI maxHPCostText;
    public TextMeshProUGUI defenseCostText;
    
    // 레벨
    public int attackLevel = 0;
    public int maxHPLevel = 0;
    public int defenseLevel = 0;

    public TextMeshProUGUI attackLevelText;
    public TextMeshProUGUI maxHPLevelText;
    public TextMeshProUGUI defenseLevelText;
    
    // 옵저버 패턴
    private PlayerStats playerStats;
    private ResourceManager resourceInfo;

    void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        resourceInfo = ResourceManager.Instance;
    }
    
    void Start()
    {
        UpdateCostUI();
        
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
        Debug.Log("1. playerAttack : " + playerStats.attack);
        if (resourceInfo.current_Coin >= attackCost)
        {
            Debug.Log("Attack Upgrade");
            playerStats.attack += attackIncrease;
            Debug.Log("2. playerAttack : " + playerStats.attack);
            resourceInfo.current_Coin -= attackCost;

            // 공격력 + 2
            totalAttackIncrease += attackIncrease;
            // 골드 + 1
            attackCost++;
            // 레벨 + 1
            attackLevel++;
            
            UpdateCostUI();
            UpdateStatsUI();
            UpdateLevelUI();
        }
        else // 경고 -> string 색 빨강으로 변경 
        {
            // 이렇게가 아니고, 버튼 클릭 -> UI / 전투력 반영, 보유 코인 반영, 코인 버튼 색 업데이트
            Debug.Log("Coin 부족");
            attackCostText.color = Color.red;
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
            UpdateGoldDisplay();
            UpdateStatsUI();
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
            UpdateGoldDisplay();
            UpdateStatsUI();
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
    
    // 관찰자 연결 활성화, 비활성화
    void OnEnable()
    {
        if (resourceInfo)
            Attach(resourceInfo);
    }

    void OnDisable()
    {
        
    }
}
