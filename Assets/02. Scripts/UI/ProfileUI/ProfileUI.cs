using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileUI : MonoBehaviour
{
    protected UserInfoManager userInfo;

    public int currentExp;
    public int levelUpExp;
    public int currentLevel;

    public TextMeshProUGUI currentLevelText;
    public Slider expbar;
    
    void Start()
    {
        userInfo = UserInfoManager.Instance;
        UserInfoManager.OnLevelUp += UpdateUI;
        
        UpdateUI();
    }
    
    void OnDestroy()
    {
        UserInfoManager.OnLevelUp -= UpdateUI;
    }

    public void UpdateUI()
    {
        if (userInfo != null)
        {
            currentExp = userInfo.currentExp;
            levelUpExp = userInfo.levelUpExp;
            currentLevel = userInfo.currentLevel;
        
            expbar.value = (float)currentExp / levelUpExp;
            currentLevelText.text = currentLevel.ToString();
        }
    }
    
}
