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

    [SerializeField] private float duration = 0.5f; // 골드 카운트 애니메이션 걸리는 시간
    private bool isInitialized = false; // 게임 시작 - 골드 애니메이션 x 위한 플래그    
    
    void Start()
    {
        playerEnforce = (PlayerEnforce)FindObjectOfType(typeof(PlayerEnforce));
        player = FindObjectOfType<PlayerController>();
        
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
        isInitialized = true; // 게임 시작 처음 ! 
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
            int newCoin = ResourceManager.Instance.current_Coin;
            
            rubyText.text = ruby.ToString();
            
            if (isInitialized && newCoin != coin) // 코인 값이 변경되었을 때, 코인 카운트 애니메이션 
            {
                StartCoroutine(CountAnimation(newCoin, coin));
            }
            else // 게임 시작 : 애니메이션 없이 바로 설정
            {
                coinText.text = Utilities.FormatNumberUnit(newCoin);
            }
        
            coin = newCoin; // 최종 값 업데이트
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

    // 숫자 카운팅 애니메이션
    IEnumerator CountAnimation(float target, float current)
    {
        float offset = (target - current) / duration; // 시간 간격마다 증가해야 할 값 계산
    
        while (current < target) // 골드가 증가하는 경우
        {
            current += offset * Time.deltaTime; // 현재 값을 프레임마다 조금씩 증가 
            coinText.text = Utilities.FormatNumberUnit((int)current);
            yield return null;
        }
    }
}
