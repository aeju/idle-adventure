using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine;
using Spine.Unity;
using DG.Tweening;

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
    
    public void TakeDamage(int damage)
    {
        Current_HP -= damage;
        ShowDamageText(damage);
        AnimateDamageText(); // 텍스트 애니메이션
    }
    
    private void ShowDamageText(int damage)
    {
        if (monsterDamageText != null)
        {
            monsterDamageText.text = damage.ToString(); 
            StartCoroutine(DisplayDamage()); // 1초동안 
        }
    }
    
    private IEnumerator DisplayDamage()
    {
        monsterDamageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        monsterDamageText.gameObject.SetActive(false);
    }
    
    public void AnimateDamageText()
    {
        // startPosition - peackPoint(대각선 위쪽) - endPoint(대각선 아래)
        Vector3 startPosition = monsterDamageText.transform.localPosition;
        Vector3 peakPoint = startPosition + new Vector3(-1, 0.5f, 0);
        Vector3 endPoint = startPosition + new Vector3(-2, -0.5f, 0);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(monsterDamageText.transform.DOLocalMove(peakPoint, 0.3f).SetEase(Ease.OutQuad))
            .Append(monsterDamageText.transform.DOLocalMove(endPoint, 0.3f).SetEase(Ease.InQuad));
    }
}
