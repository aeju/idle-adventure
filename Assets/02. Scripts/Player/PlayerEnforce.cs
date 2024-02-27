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
public class PlayerEnforce : EnforceSubject
{
    // 버튼
    [SerializeField] private Button attackBtn;
    [SerializeField] private Button maxHPBtn;
    [SerializeField] private Button defenseBtn;
    
    // 증가량, 변수 이름 수정 필요 
    public int attack;
    public int hp;
    public int defense;

    // 골드 비용
    public int attackCost;
    public int maxHPCost;
    public int defenseCost;

    public TextMeshProUGUI attackCostText;
    public TextMeshProUGUI maxHPCostText;
    public TextMeshProUGUI defenseCostText;
    
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
        attackCostText.text = attackCost.ToString();
        maxHPCostText.text = maxHPCost.ToString();
        defenseCostText.text = defenseCost.ToString();
    }

    // + 5 <- 변수로 변경 
    private void UpgradeAttack()
    {
        Debug.Log("1. playerAttack : " + playerStats.attack);
        if (resourceInfo.current_Coin >= maxHPCost)
        {
            Debug.Log("Attack Upgrade");
            playerStats.attack += 5;
            Debug.Log("2. playerAttack : " + playerStats.attack);
            resourceInfo.current_Coin -= maxHPCost;
            UpdateGoldDisplay();
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
            playerStats.maxHP += 5;
            playerStats.currentHP = playerStats.maxHP;
            // 필요: PlayerController HPSliderUpdate();
            Debug.Log("2. playerHP : " + playerStats.maxHP);
            resourceInfo.current_Coin -= maxHPCost;
            UpdateGoldDisplay();
            UpdateStatsDisplay();
        }
    }
    
    // 데미지 식에 반영 
    private void UpgradeDefense()
    {
        Debug.Log("1. playerDefense : " + playerStats.maxHP);
        if (resourceInfo.current_Coin >= defenseCost)
        {
            Debug.Log("Defense Upgrade");
            playerStats.defense += 5;
            Debug.Log("2. playerDefense : " + playerStats.defense);
            resourceInfo.current_Coin -= defenseCost;
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
