using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class PotionBtn : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI potionText;
    [SerializeField] private GameObject potionImage;
    [SerializeField] private Button potionBtn;
    [SerializeField] private int healAmount = 50;
    [SerializeField] private float potionCooldown = 3f; // 포션 사용 쿨타임
    private PlayerController playerController;
    
    private PotionManager potionManager;
    
    private void Start()
    {
        potionManager = PotionManager.Instance;
        if (potionManager == null)
        {
            Debug.LogError("PotionManager instance not found!");
            return;
        }

        // 초기 상태 설정
        UpdatePotionDisplay();

        // PotionManager의 이벤트 구독
        potionManager.OnResourcesUpdated += UpdatePotionDisplay;
        playerController = FindObjectOfType<PlayerController>();
        
        //potionBtn.OnClickAsObservable().Subscribe(_ =>
        //potionBtn.OnClickAsObservable().ThrottleFirst(TimeSpan.FromSeconds(3)).Subscribe(_ =>
        potionBtn.OnClickAsObservable().ThrottleFirst(TimeSpan.FromSeconds(potionCooldown)).Subscribe(_ =>
        {
            UsePotion();
        }).AddTo(this);
    }
    
    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (potionManager != null)
        {
            potionManager.OnResourcesUpdated -= UpdatePotionDisplay;
        }
    }

    private void UpdatePotionDisplay()
    {
        int potionCount = potionManager.GetCurrentPotions();
        
        // 포션 개수가 1 이상일 때만 텍스트, 이미지 활성화
        bool isActive = potionCount > 0;
        potionText.gameObject.SetActive(isActive); // 포션 개수 
        potionImage.SetActive(isActive); // 포션 이미지 

        // 포션 개수 텍스트 업데이트
        potionText.text = potionCount.ToString();
    }
    
    private void UsePotion()
    {
        if (potionManager.GetCurrentPotions() > 0)
        {
            // 최대 HP 넘지 않도록
            int currentHP = playerController.playerStats.CurrentHP;
            int maxHP = playerController.playerStats.maxHP;
            int newHP = Mathf.Min(currentHP + healAmount, maxHP); // 현재 HP + 회복량 vs MaxHP 중 작은값 선택
            
            playerController.playerStats.CurrentHP = newHP;
            potionManager.UsePotion(1);
            UpdatePotionDisplay();
            
            // healEffectParticle 재생
            if (playerController.healEffectParticle != null)
            {
                playerController.healEffectParticle.Play();
            }
            else
            {
                Debug.LogWarning("Heal effect particle is not assigned in PlayerController");
            }
        }
    }
}
