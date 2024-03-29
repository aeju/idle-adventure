using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 현재 경험치, 현재 레벨 
// 100 이상 획득 -> 레벨업 
public class UserInfoManager : Singleton<UserInfoManager>
{
    public int currentExp = 0; // 현재 경험치 
    public int currentLevel = 1; // 현재 레벨 
    public int levelUpExp = 10; // 레벨업에 필요한 경험치 
    
    // 프로필 UI 반영
    public delegate void LevelUpAction();
    public static event LevelUpAction OnLevelUp;
    
    public void AddExperience(int exp)
    {
        // 버프에 따른 경험치 계산을 항상 적용 (기본 = 1)
        float adjustedExp = exp * BuffManager.Instance.ExpMultiplier;
        // 정수로 변환하기 전, 반올림
        int finalExp = Mathf.RoundToInt(adjustedExp);
        
        currentExp += finalExp;
        CheckLevelUp(); 
        Debug.Log("Exp: " + exp);
        Debug.Log("BuffExp: " + finalExp);
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

    // 필요 -> 레벨업 축하 팝업 (추가 로직 필요)
    private void LevelUp()
    {
        Debug.Log($"level : {currentLevel}.");
        
        OnLevelUp?.Invoke();
    }
}
