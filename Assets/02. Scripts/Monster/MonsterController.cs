using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine;
using Spine.Unity;

public class MonsterController : MonoBehaviour
{
    public static MonsterStats monsterStats;

    public GameObject[] Player;

    // 애니메이션
    private Animator anim;
    
    // 현재 체력
    public Slider hpbar;
    public int Max_HP = 20;
    public int Current_HP = 20;

    // 공격력
    public int Combat;
    
    // 데미지
    public TextMeshProUGUI monsterDamageText;
    
    // 드랍
    public GameObject dropItem;

    private bool isDead = false;

    void Start()
    {
        monsterStats = GetComponent<MonsterStats>();

        Player = GameObject.FindGameObjectsWithTag("player");

        anim = GetComponent<Animator>();

        hpbar.value = Current_HP / Max_HP;
    }
    
    void Update()
    {
        MonsterHPSlider();
        if (Current_HP <= 0 && !isDead)
        {
            MonsterDeath();
            isDead = true;
        }
            
    }

    void MonsterDeath()
    {
        anim.SetTrigger("Dead");
        ItemDrop();
        //Instantiate(dropItem, transform.position, Quaternion.identity);
    }

    // 지면으로 띄우기 
    void ItemDrop()
    {
        Vector3 dropPosition = transform.position + new Vector3(0, 1.0f, 0);
        Instantiate(dropItem, dropPosition, Quaternion.identity);
    }

    void MonsterHPSlider()
    {
        hpbar.value = Mathf.Lerp((float) hpbar.value, (float)Current_HP / (float)Max_HP, Time.deltaTime * 10);
    }
}
