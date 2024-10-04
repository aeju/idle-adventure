using UnityEngine;
using TMPro;
using DG.Tweening;

public struct TextAttributes
{
    public Color TextColor;
    public readonly float FontSize;

    public TextAttributes(Color textColor, float fontSize)
    {
        TextColor = textColor;
        FontSize = fontSize;
    }
}

// 생성되고 일정 시간 뒤에 파괴되도록 
public class DamageText : MonoBehaviour
{
    private TextMeshPro damagetext;
    private EnemyFSM enemyFSM;

    private PlayerController playerController;

    public int damage;
    private Color textColor;

    // 애니메이션 경로
    // 오른쪽 : (-1, 0.5, 0) -> (-2, -0.5, 0)
    // 왼쪽 : (1, 0.5, 0) -> (2, -0.5, 0)
    private const float RISE_X_OFFSET = 1f;
    private const float RISE_Y_OFFSET = 0.5f;
    private const float FALL_X_OFFSET = 2f;
    private const float FALL_Y_OFFSET = -0.5f;
    
    [SerializeField] private float maxScaleMultiplier = 1.8f;
    [SerializeField] private float riseDuration = 0.3f; // 시작점 - 정점 지속 시간
    [SerializeField] private float fallDuration = 0.3f; // 정점 - 끝점 지속 시간
    
    private float totalDuration; // 전체 애니메이션 지속 시간
    
    void Start()
    {
        damagetext = GetComponent<TextMeshPro>();
        damagetext.text = damage.ToString();
        
        SetDamageTextProperties();

        totalDuration = riseDuration + fallDuration;
    }
    
    private void SetDamageTextProperties()
    {
        TextAttributes attributes = new TextAttributes(); // 기본 속성

        enemyFSM = GetComponentInParent<EnemyFSM>(); 
        playerController = GetComponentInParent<PlayerController>();

        if (enemyFSM != null)
        {
            attributes = new TextAttributes(Color.red, 14f);
            AnimateDamageText(enemyFSM.flipX); 
        }
        else if (playerController != null) // Player : scale = 2 라서  
        {
            attributes = new TextAttributes(Color.blue, 7.5f);
            AnimateDamageText(playerController.isFlipX);
        }
        // 속성 적용
        damagetext.color = attributes.TextColor;
        damagetext.fontSize = attributes.FontSize;
    }
    
    public void InitializeDamageText(bool isFlipX)
    {
        TextMeshPro textMesh = GetComponent<TextMeshPro>();
        textMesh.text = damage.ToString(); // damage 변수 값을 문자열로 변환 
        AnimateDamageText(isFlipX); // 데미지 텍스트 애니메이션 
    }
    
    public void AnimateDamageText(bool isFacingRight)
    {
        // flipX 상태에 따라 데미지 텍스트 애니메이션 방향 결정
        float directionMultiplier = isFacingRight ? -1f : 1f; // 오른쪽을 바라보면 -1, 왼쪽이면 1

        Vector3 startPosition = transform.localPosition;
        Vector3 peakPoint = startPosition + new Vector3(RISE_X_OFFSET * directionMultiplier, RISE_Y_OFFSET, 0);
        Vector3 endPoint = startPosition + new Vector3(FALL_X_OFFSET * directionMultiplier, FALL_Y_OFFSET, 0);

        Vector3 originalScale = transform.localScale; // 원래 크기 
        Vector3 peakScale = originalScale * maxScaleMultiplier; // 최대 크기 
        
        Sequence sequence = DOTween.Sequence();
        
        // 1. 시작점 -> 정점
        sequence.Append(transform.DOLocalMove(peakPoint, riseDuration).SetEase(Ease.OutQuad)); // 위치 : 올라가기 
        sequence.Join(transform.DOScale(peakScale, riseDuration).SetEase(Ease.OutQuad)); // 크기 증가
    
        // 2. 정점 -> 끝점
        sequence.Append(transform.DOLocalMove(endPoint, fallDuration).SetEase(Ease.InQuad)); // 위치 : 내려가기 
        sequence.Join(transform.DOScale(originalScale, fallDuration).SetEase(Ease.InQuad)); // 크기 원래대로 
        sequence.Join(damagetext.DOFade(0, fallDuration).SetEase(Ease.InQuad)); // Fade-out 
        
        sequence.OnComplete(() => Destroy(gameObject, totalDuration)); // 애니메이션 종료 후 객체 파괴
    }
}
