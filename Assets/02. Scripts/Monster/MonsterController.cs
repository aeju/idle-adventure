using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine;
using Spine.Unity;
using AnimationState = Spine.AnimationState;

public class MonsterController : MonoBehaviour
{
    private MonsterStats monsterStats;

    public GameObject[] Player;
    
    // 애니메이션
    public Animator anim;
    //public SkeletonAnimation skeletonAnimation;
    //public AnimationState animationState;

    public string idleAnim = "Idle";
    public string walkAnim = "Walk";
    public string attackAnim = "Attack";
    public string deathAnim = "Dead";
    
    // 현재 체력
    public int Current_HP = 20;
    
    // 공격력
    public int Combat;
    
    private Animator animator;
    
    void Start()
    {
        monsterStats = GetComponent<MonsterStats>();

        Player = GameObject.FindGameObjectsWithTag("player");

        anim = GetComponent<Animator>();

        //animationState = skeletonAnimation.AnimationState;
    }

    // Update is called once per frame
    void Update()
    {
        if (Current_HP <= 0)
            MonsterDeath();
    }

    void MonsterDeath()
    {
        anim.SetTrigger("Dead");
        //animationState.SetAnimation(0, deathAnim, false);
        //Debug.Log("Death");
        // Check if the animation exists in the skeleton data
        //if (skeletonAnimation.Skeleton.Data.FindAnimation(deathAnim) != null)
        {
            // Set the animation to the death animation
            //animationState.SetAnimation(0, deathAnim, false);
            //Debug.Log("Death");
        }
        //else
        {
            //Debug.LogWarning("Death animation not found in skeleton data.");
        }
    }
}
