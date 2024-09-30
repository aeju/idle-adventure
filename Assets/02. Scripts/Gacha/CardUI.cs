using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using TMPro;

public class CardUI : MonoBehaviour
{
    public Image characterImage;
    public Image backImage;
    public Image frontImage;
    public TextMeshProUGUI characterNameText;
    public bool IsFlipped { get; private set; }
    
    private Animator animator;
    
    [SerializeField] private Sprite normalBackSprite;
    [SerializeField] private Sprite epicBackSprite;
    [SerializeField] private Sprite uniqueBackSprite;
    [SerializeField] private Sprite legendBackSprite;
    
    [SerializeField] private Sprite normalFrontSprite;
    [SerializeField] private Sprite epicFrontSprite;
    [SerializeField] private Sprite uniqueFrontSprite;
    [SerializeField] private Sprite legendFrontSprite;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    // 카드의 정보를 초기화
    public void CardUISet(Card card)
    {
        characterImage.sprite = card.cardImage;
        characterNameText.text = card.cardName;
        
        // 등급에 따라 카드 뒷면 이미지 설정
        switch (card.rarity)
        {
            case Definitions.CardRarity.Normal:
                backImage.sprite = normalBackSprite;
                frontImage.sprite = normalFrontSprite;
                break;
            case Definitions.CardRarity.Epic:
                backImage.sprite = epicBackSprite;
                frontImage.sprite = epicFrontSprite;
                break;
            case Definitions.CardRarity.Unique:
                backImage.sprite = uniqueBackSprite;
                frontImage.sprite = uniqueFrontSprite;
                break;
            case Definitions.CardRarity.Legend:
                backImage.sprite = legendBackSprite;
                frontImage.sprite = legendFrontSprite;
                break;
        }
    }

    // randomselect에서 카드 생성 후, 호출
    public void FlipCard()
    {
        animator.SetTrigger("Flip");
        IsFlipped = true; // 뒤집힌 상태로 전환
    }
}
