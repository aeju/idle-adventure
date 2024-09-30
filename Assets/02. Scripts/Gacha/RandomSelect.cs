using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class RandomSelect : MonoBehaviour
{
    public List<Card> deck = new List<Card>();  // 카드 덱
    private int total = 0;  // 카드들의 가중치 총 합
    [SerializeField] private GameObject panel; // 카드를 표시할 패널
    [SerializeField] private Transform parent; // 카드를 생성할 부모 Transform (panel의 자식)
    
    [SerializeField] private GameObject cardprefab;
    
    [SerializeField] private Button Btn1;
    [SerializeField] private Button Btn10;
    [SerializeField] private Button Btn30;
    
    public List<Card> result = new List<Card>();  // 랜덤하게 선택된 카드를 담을 리스트
    
    private Coroutine flipCoroutine; // 코루틴을 저장할 변수
    public float cardFlipDelay = 0.2f;
    private WaitForSeconds waitTime;

    private bool isFlipping = false; // 카드가 뒤집히는 중인지 확인하는 플래그
    private bool allCardsFlipped = false; // 모든 카드가 뒤집혔는지 확인하는 플래그
    
    void Awake()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            total += deck[i].weight; // 카드 덱의 모든 카드의 총 가중치를 구해주기
        }
    }
    
    void Start()
    {
        waitTime = new WaitForSeconds(cardFlipDelay); // 대기 시간 객체 생성 
        
        // 1회 소환 
        Btn1.OnClickAsObservable().Subscribe(_ =>
        {
            ResultSelect(1);
        }).AddTo(this);
        
        // 10회 소환
        Btn10.OnClickAsObservable().Subscribe(_ =>
        {
            ResultSelect(10);
        }).AddTo(this);
        
        // 30회 소환
        Btn30.OnClickAsObservable().Subscribe(_ =>
        {
            ResultSelect(30);
        }).AddTo(this);
        
        // 처음 : 패널 비활성화
        panel.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isFlipping)
            {
                FlipAllCardsImmediately();
            }
            else if (allCardsFlipped)
            {
                HidePanel();
            }
        }
        /*
        // 카드가 뒤집히는 중 + 터치 입력이 있을 때
        if (isFlipping && (Input.GetMouseButtonDown(0)))
        {
            FlipAllCardsImmediately();
        }
        */
    }
    
    public void ResultSelect(int count)
    {
        // 패널 활성화
        panel.SetActive(true);
        
        // 기존 코루틴이 실행 중이라면 중지
        if (flipCoroutine != null)
        {
            StopCoroutine(flipCoroutine);
        }
        
        // 기존 결과 및 생성된 카드 UI 초기화
        result.Clear();
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < count; i++) // 버튼에 따라, 소환횟수 다르게 
        {
            // 가중치 랜덤을 돌리면서 결과 리스트에 넣어주기
            Card selectedCard = RandomCard();
            if (selectedCard != null)
            {
                result.Add(selectedCard); // 비어 있는 카드 생성
                CardUI cardUI = Instantiate(cardprefab, parent).GetComponent<CardUI>(); // parent에 생성
                cardUI.CardUISet(selectedCard); // 생성된 카드에 결과 리스트의 정보를 넣어주기
            }
        }
        
        // 순차적으로 카드 뒤집기 시작
        flipCoroutine = StartCoroutine(SequentialCardFlip());
    }
    
    // 가중치 랜덤
    public Card RandomCard()
    {
        int weight = 0;
        int selectNum = 0;

        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));

        for (int i = 0; i < deck.Count; i++)
        {
            weight += deck[i].weight;
            if (selectNum <= weight)
            {
                Card temp = new Card(deck[i]);
                return temp;
            }
        }
        return null;
    }
    
    private IEnumerator SequentialCardFlip()
    {
        isFlipping = true;
        allCardsFlipped = false;
        CardUI[] cardUIs = parent.GetComponentsInChildren<CardUI>();
        foreach (CardUI cardUI in cardUIs)
        {
            yield return waitTime; // 각 카드 사이의 딜레이 (캐시된 WaitForSeconds 객체 사용)
            cardUI.FlipCard();
        }
        
        // 코루틴이 완료되면 변수 초기화
        flipCoroutine = null;
        isFlipping = false;
        
        allCardsFlipped = true; // 모든 카드 뒤집힘
    }
    
    private void FlipAllCardsImmediately()
    {
        if (flipCoroutine != null)
        {
            StopCoroutine(flipCoroutine); // 코루틴 중단
        }

        CardUI[] cardUIs = parent.GetComponentsInChildren<CardUI>();
        foreach (CardUI cardUI in cardUIs)
        {
            if (!cardUI.IsFlipped)
            {
                cardUI.FlipCard();
            }
        }

        isFlipping = false;
        flipCoroutine = null;
        
        allCardsFlipped = true;
    }
    
    private void HidePanel()
    {
        // 모든 카드 삭제
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        // 패널 비활성화
        panel.SetActive(false);

        // 상태 초기화
        allCardsFlipped = false;
        result.Clear();
    }
}
