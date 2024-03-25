using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using UniRx;

public class RewardAdManager : MonoBehaviour
{
    public bool isAdWatched; // 광고 시청 여부 
    
    [SerializeField] private GameObject rewardPopup; 
    
    [SerializeField] Button rewardADBtn;
    // 광고 테스트 ID
    [SerializeField] private readonly string rewardedAdUnitId = "ca-app-pub-9942110624413430/2454306700";
    
    // 광고 변수
    RewardedAd rewardedAd;
    AdRequest adRequest;
    
    // 버프 활성화 이벤트
    public static event Action OnBuffActivated;
    
    void Start()
    {
        isAdWatched = false;
        PopupOff();
        
        // 초기화
        MobileAds.Initialize(initStatus => { });
        adRequest = new AdRequest.Builder().Build();

        // 보상형 광고를 미리 로드
        LoadRewardedAd();
        
        // rewardADBtn 버튼 - 클릭 리스너 추가
        rewardADBtn.onClick.AddListener(ShowRewardedAd);
    }

    
    void LoadRewardedAd(bool show = false)
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        // 보상형 광고를 로드
        RewardedAd.Load(rewardedAdUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load an ad with error: " + error);
                return;
            }

            Debug.Log("Rewarded ad loaded with response: " + ad.GetResponseInfo());

            rewardedAd = ad;

            // show 매개변수가 true로 설정되어 있으면 광고를 표시
            if (show)
            {
                rewardedAd.Show(RewardHandler);
            }
        });
    }

    void ShowRewardedAd()
    {
        // 보상형 광고가 로드되었고, 광고를 표시할 수 있는 상태인지 확인
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            // 보상형 광고 표시
            rewardedAd.Show(RewardHandler);
        }
        else
        {
            // 보상형 광고를 로드하지 않았거나 광고를 표시할 수 없는 상태라면 다시 로드
            LoadRewardedAd(true);
        }
    }

    // 콜백 함수 (광고를 본 후 보상 처리)
    void RewardHandler(Reward reward)
    {
        if (rewardPopup != null)
        {
            isAdWatched = true;
        }
        else
        {
            Debug.LogError("rewardPopupActive is null");
        }
    }

    void Update()
    {
        if (isAdWatched) // 닫힐 때 바뀌도록
        {
            PopupOn(); 
            Debug.Log("isAdWatched");
        }

        else
        {
            PopupOff();
        }
    }
    
    void PopupOn()
    {
        if (rewardPopup != null)
        {
            rewardPopup.SetActive(true);
        }
    }

    void PopupOff()
    {
        if (rewardPopup != null)
        {
            rewardPopup.SetActive(false);
        }
    }
}
