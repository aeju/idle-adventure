using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 시간 초 재서 
// 
public class CoinBuff : MonoBehaviour
{
    private ResourceManager resourceInfo;
    
    // 원하는 시간 동안, 코인 획득 % 늘리기 
    // 코인획득 : EnemyFSM - EnemyRewards() 
    void EarnRewards()
    {
        if (resourceInfo != null)
        {
            //resourceInfo.AddCoin(monsterStats.coin);
        }
    }
}
