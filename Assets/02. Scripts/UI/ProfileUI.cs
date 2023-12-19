using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileUI : MonoBehaviour
{
    protected UserInfo userInfo;

    public int currentExp;
    public int levelUpExp;
    public int currentLevel;

    public TextMeshProUGUI currentLevelText;
    public Slider expbar;
    
    void Start()
    {
        userInfo = UserInfo.Instance;
        
        currentExp = userInfo.currentExp;
        levelUpExp = userInfo.levelUpExp;
        currentLevel = userInfo.currentLevel;
        
        expbar.value = (float)currentExp / levelUpExp;
        currentLevelText.text = currentLevel.ToString();
    }
}
