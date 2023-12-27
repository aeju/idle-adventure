using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    //public TextMeshProUGUI monsterDamageText;
    
    // 드랍
    public GameObject dropItem;
    
    // 경험치
    public int expReward = 50;
    protected UserInfoManager userInfo;
    // 금화
    public int coinReward = 1000;
    protected ResourceManager resourceInfo;

    private bool isDead = false;

    private DamageTextAnimator damageTextAnimator;
    public GameObject hudDamageText;
    
    void Start()
    {
        monsterStats = GetComponent<MonsterStats>();

        Player = GameObject.FindGameObjectsWithTag("player");

        anim = GetComponent<Animator>();

        hpbar.value = Current_HP / Max_HP;
        
        userInfo = UserInfoManager.Instance;
        resourceInfo = ResourceManager.Instance;
        damageTextAnimator = FindObjectOfType<DamageTextAnimator>();
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
    
    // 지면으로 띄우기 
    void ItemDrop()
    {
        //Vector3 dropPosition = transform.position + new Vector3(0, 1.0f, 0);
        //Instantiate(dropItem, dropPosition, Quaternion.identity);
        Vector3 dropPosition = transform.position + new Vector3(0, 1.0f, 0);
        GameObject droppedItem = Instantiate(dropItem, dropPosition, Quaternion.identity);

        StartCoroutine(MoveItemToPlayer(droppedItem));
    }
    
    IEnumerator MoveItemToPlayer(GameObject item)
    {
        float duration = 1.0f; // 이동 
        Vector3 playerPosition = Player[1].transform.position; // Player-prefab


        Tween moveTween = item.transform.DOMove(playerPosition, duration).SetEase(Ease.InOutQuad);
        yield return moveTween.WaitForCompletion();
        Destroy(item); // or item.SetActive(false) 
    }

    void MonsterHPSlider()
    {
        hpbar.value = Mathf.Lerp((float) hpbar.value, (float)Current_HP / (float)Max_HP, Time.deltaTime * 10);
    }
    
    public void TakeDamage(int damage)
    {
        Debug.Log($"Monster taking damage: {damage}, Current HP before: {Current_HP}");

        Current_HP -= damage;
        Debug.Log($"Current HP after damage: {Current_HP}");

        if (hudDamageText != null)
        {
            GameObject damageText = Instantiate(hudDamageText);
            damageText.GetComponent<DamageText>().damage = damage;
            
            //damageTextAnimator.ShowDamageText(damage, transform.position);
        }
        else
        {
            Debug.LogError("DamageTextAnimator is null.");
        }
        //ShowDamageText(damage); // 텍스트 띄우기 
        //AnimateDamageText(); // 텍스트 애니메이션
    }
    
    void MonsterDeath()
    {
        anim.SetTrigger("Dead");
        ItemDrop();

        Debug.Log("Monster Death");
        if (userInfo != null)
        {
            userInfo.AddExperience(expReward);
            Debug.Log("Add Exp");
        }

        if (resourceInfo != null)
        {
            resourceInfo.AddCoin(coinReward);
            Debug.Log("Add Coin");
        }
    }
    
    /*
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
    
    // startPosition - peackPoint(대각선 위쪽) - endPoint(대각선 아래)
    public void AnimateDamageText()
    {
        Vector3 startPosition = monsterDamageText.transform.localPosition;
        Vector3 peakPoint = startPosition + new Vector3(-1, 0.5f, 0);
        Vector3 endPoint = startPosition + new Vector3(-2, -0.5f, 0);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(monsterDamageText.transform.DOLocalMove(peakPoint, 0.3f).SetEase(Ease.OutQuad))
            .Append(monsterDamageText.transform.DOLocalMove(endPoint, 0.3f).SetEase(Ease.InQuad));
    }
    */
}
