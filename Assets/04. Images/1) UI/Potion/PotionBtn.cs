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
    //[SerializeField] private int healAmount = 50;
    [SerializeField] private float potionCooldown = 3f; // 포션 사용 쿨타임
    private PlayerController playerController;
    
    private PotionManager potionManager;
    
    private void Start()
    {
        potionManager = PotionManager.Instance;
        playerController = FindObjectOfType<PlayerController>();
        
        if (potionManager == null)
        {
            Debug.LogError("PotionManager instance not found!");
            return;
        }

        // 초기 상태 설정
        UpdatePotionDisplay();

        // PotionManager의 이벤트 구독
        potionManager.OnResourcesUpdated += UpdatePotionDisplay;
        
        potionBtn.OnClickAsObservable().ThrottleFirst(TimeSpan.FromSeconds(potionCooldown)).Subscribe(_ =>
        {
            playerController.UsePotion();
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
}
