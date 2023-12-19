using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            Debug.LogError("ResourceManager instance is null!");
            return;
        }
        
        UpdateUI();
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
            coinText.text = coin.ToString();
        }
    }
}
