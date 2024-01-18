using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 스크립트의 데미지 처리 함수 실행
public class MonsterHitEvent : MonoBehaviour
{
    public EnemyFSM efsm;

    public void PlayerHit()
    {
        efsm.AttackAction();
    }
}
