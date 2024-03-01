using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 루비, 코인(단위!) / 뽑기 = 카드 
// 전투력 
// 드랍 아이템 -> 리소스바 UI 반영 

// 추가 작업 필요: 전투력 변화 -> 업데이트
public class ResourceBar : EnforceObserver 
{
    protected ResourceManager resoureInfo;
    public PlayerController player;
    
    private PlayerEnforce playerEnforce;
    
    public int ruby;
    public int coin;

    //public int summon_Ticket;

    public TextMeshProUGUI rubyText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI combatPowerText;

    void Start()
    {
        resoureInfo = ResourceManager.Instance;
        playerEnforce = (PlayerEnforce) FindObjectOfType(typeof(PlayerEnforce));

        if (resoureInfo == null)
        {
            Debug.LogError("Resource Manager null");
            return;
        }
        
        if (player == null)
        {
            Debug.LogError("Player Controller null");
            return;
        }
        
        resoureInfo.OnResourcesUpdated += UpdateUI; // 이벤트 구독 
        UpdateUI();
    }

    void OnDestroy()
    {
        if (resoureInfo != null)
        {
            resoureInfo.OnResourcesUpdated -= UpdateUI; // 구독 해제
        }
    }
    
    void UpdateUI()
    {
        ResourceUpdate(); // 리소스: 루비, 코인
        CombatPowerUpdate(); // 전투력
    }

    void ResourceUpdate()
    {
        if (resoureInfo != null)
        {
            // 루비, 코인, 티켓 null 처리 
            ruby = resoureInfo.current_Ruby;
            coin = resoureInfo.current_Coin;

            rubyText.text = ruby.ToString();
            coinText.text = NumberFormatter.FormatNumberUnit(coin);
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
            combatPowerText.text = NumberFormatter.FormatNumberUnit(combatPower);
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
