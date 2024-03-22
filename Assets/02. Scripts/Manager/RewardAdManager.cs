using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using UniRx;

public class RewardAdManager : MonoBehaviour
{
    [SerializeField] private GameObject rewardPopup; 
    
    [SerializeField] Button rewardADBtn;
    // 광고 테스트 ID
    [SerializeField] private readonly string rewardedAdUnitId = "ca-app-pub-9942110624413430/2454306700";
    
    // 광고 변수
    RewardedAd rewardedAd;
    AdRequest adRequest;
    
    // 버프 활성화 이벤트
    public static event Action OnBuffActivated;
    
    
    void Stat()
    {
        // 초기화
        MobileAds.Initialize(initStatus => { });
        adRequest = new AdRequest.Builder().Build();

        // 보상형 광고를 미리 로드
        LoadRewardedAd();
        
        
        // rewardADBtn 버튼 - 클릭 리스너 추가
        rewardADBtn.onClick.AddListener(ShowRewardedAd);
        
        /*
        // 팝업부터 띄우기
        if (rewardPopup != null)
        {
            rewardPopup.SetActive(false);
        }
        */
    }

    void OnEnable()
    {
        Debug.Log("1. rewardPopup.SetActive(false);");
        // 팝업 끄기 
        if (rewardPopup != null)
        {
            Debug.Log("2. rewardPopup.SetActive(false);");
            rewardPopup.SetActive(false);
        }
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

    public void ShowRewardedAd()
    {
        Debug.Log("Btn Click1");
        // 보상형 광고가 로드되었고, 광고를 표시할 수 있는 상태인지 확인
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            // 보상형 광고 표시
            rewardedAd.Show(RewardHandler);
            Debug.Log("Btn Click2");
        }
        else
        {
            // 보상형 광고를 로드하지 않았거나 광고를 표시할 수 없는 상태라면 다시 로드
            LoadRewardedAd(true);
            Debug.Log("Btn Click4");
        }
    }

    // 콜백 함수 (광고를 본 후 보상 처리)
    void RewardHandler(Reward reward)
    {
        Debug.Log($"RewardHandler called. Popup active state before set: {rewardPopup.activeSelf}");
        Debug.Log("Btn Click5");
        //Debug.Log($"RewardHandler called. Popup active state before set: {rewardPopup.activeSelf}");
        rewardPopup.SetActive(true);
        /*
        // 팝업부터 띄우기
        if (rewardPopup != null)
        {
            rewardPopup.SetActive(true);
        }*/
        
        /*
        // CoinBuff 활성화
        if (coinBuff != null)
        {
            // 보상 -> 코루틴 X, 중간 한 단계 더 필요 
            coinBuff.Activate();
            
            // 버프 활성화 이벤트 발생
            OnBuffActivated?.Invoke();
        }
        */
    }
}
