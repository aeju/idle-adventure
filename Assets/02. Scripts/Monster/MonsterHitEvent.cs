using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitEvent : MonoBehaviour
{
    public EnemyFSM efsm;

    public void PlayerHit()
    {
        efsm.AttackAction();
    }
}
