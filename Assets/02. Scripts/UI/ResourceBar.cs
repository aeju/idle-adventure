using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 루비, 코인(단위!) / 뽑기 
// 전투력 
// 드랍 아이템 -> 리소스바 UI 반영 
public class ResourceBar : MonoBehaviour
{
    protected ResourceManager resoureInfo;
    
    public int ruby;
    public int coin;
    public int summon_Ticket;

    public TextMeshProUGUI rubyText;
    public TextMeshProUGUI coinText;
    
    void Start()
    {
        resoureInfo = ResourceManager.Instance;
        
        if (resoureInfo == null)
        {
            Debug.LogError("ResourceManager null");
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
        if (resoureInfo != null)
        {
            // 루비, 코인, 티켓 null 처리 
            ruby = resoureInfo.current_Ruby;
            coin = resoureInfo.current_Coin;
            summon_Ticket = resoureInfo.current_summon_Ticket;

            rubyText.text = ruby.ToString();
            //coinText.text = coin.ToString();
            coinText.text = FormatCoinUnit(coin);
        }
    }

    // K : 1,000
    // M : 1,000,000
    // B : 1,000,000,000
    string FormatCoinUnit(int number)
    {
        if (number >= 1000)
        {
            return (number / 1000f).ToString("0.#") + "K";
        }
        
        else if (number >= 1000000)
        {
            return (number / 1000000f).ToString("0.#") + "M";
        }
        
        else
        {
            return number.ToString();
        }
    }
}
