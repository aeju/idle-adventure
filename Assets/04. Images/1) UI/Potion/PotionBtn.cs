using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionBtn : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI potionText;
    [SerializeField] private GameObject potionImage;
    
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
        //potionManager.OnPotionUpdated += UpdatePotionDisplay;
        potionManager.OnResourcesUpdated += UpdatePotionDisplay;
    }
    
    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (potionManager != null)
        {
            //potionManager.OnPotionUpdated -= UpdatePotionDisplay;
            potionManager.OnResourcesUpdated -= UpdatePotionDisplay;
        }
    }

    private void UpdatePotionDisplay()
    {
        //int potionCount = potionManager.current_Potion;
        //int potionCount = potionManager.currentResource;
        int potionCount = potionManager.GetCurrentResource();
        
        // 포션 개수가 1 이상일 때만 텍스트, 이미지 활성화
        bool isActive = potionCount > 0;
        potionText.gameObject.SetActive(isActive); // 포션 개수 
        potionImage.SetActive(isActive); // 포션 이미지 

        // 포션 개수 텍스트 업데이트
        potionText.text = potionCount.ToString();
    }
}
