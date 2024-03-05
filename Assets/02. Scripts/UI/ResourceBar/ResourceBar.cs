using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 루비, 코인(단위!) / 뽑기 = 카드 
// 전투력 
// 드랍 아이템 -> 리소스바 UI 반영 
public class ResourceBar : EnforceObserver 
{
    private PlayerController player;
    private PlayerEnforce playerEnforce;
    
    private int ruby;
    private int coin;
    //public int summon_Ticket;

    [SerializeField] private TextMeshProUGUI rubyText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI combatPowerText;

    void Start()
    {
        playerEnforce = (PlayerEnforce)FindObjectOfType(typeof(PlayerEnforce));

        if (ResourceManager.Instance == null)
        {
            Debug.LogError("Resource Manager null");
            return;
        }
        
        if (player == null)
        {
            Debug.LogError("Player Controller null");
            return;
        }
        
        ResourceManager.Instance.OnResourcesUpdated += UpdateUI; // 리소스 매니저 이벤트 직접 구독
        UpdateUI();
    }

    void OnDestroy()
    {
        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.OnResourcesUpdated -= UpdateUI; // 구독 해제
        }
    }
    
    void UpdateUI()
    {
        ResourceUpdate(); // 리소스: 루비, 코인
        CombatPowerUpdate(); // 전투력
    }

    // 리소스 업데이트
    void ResourceUpdate()
    {
        if (ResourceManager.Instance != null)
        {
            ruby = ResourceManager.Instance.current_Ruby;
            coin = ResourceManager.Instance.current_Coin;
            
            rubyText.text = ruby.ToString();
            coinText.text = Utilities.FormatNumberUnit(coin);
        }
    }

    void CombatPowerUpdate()
    {
        if (player != null)
        {
            int attack = player.playerStats.attack;
            int maxHP = player.playerStats.maxHP;
            int defense = player.playerStats.defense;

            // 전투력 : 다시 계산
            int combatPower = CombatCalculator.CalculateCombatPower(attack, maxHP, defense);
            combatPowerText.text = Utilities.FormatNumberUnit(combatPower);
        }
    }
    
    public override void Notify(EnforceSubject subject)
    {
        if (!playerEnforce)
            playerEnforce = subject.GetComponent<PlayerEnforce>();

        if (playerEnforce)
            UpdateUI();
    }
}
