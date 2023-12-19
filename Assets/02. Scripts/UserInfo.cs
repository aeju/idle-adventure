using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 현재 경험치, 현재 레벨 
// 100 이상 획득 -> 레벨업 

public class UserInfo : MonoBehaviour
{
    public static UserInfo Instance { get; private set; }

    public int currentExp = 0; // 현재 경험치 
    public int currentLevel = 1; // 현재 레벨 
    public int levelUpExp = 10; // 레벨업에 필요한 경험치 
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void AddExperience(int exp)
    {
        currentExp += exp;
        CheckLevelUp(); 
    }

    private void CheckLevelUp()
    {
        while (currentExp >= levelUpExp)
        {
            currentExp -= levelUpExp;
            currentLevel++;
            LevelUp();
        }
    }

    // 필요 -> 레벨업 축하 팝업
    private void LevelUp()
    {
        Debug.Log($"Congratulations! You've reached level {currentLevel}.");
    }
}
