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

    void Start()
    {
        damagetext = GetComponent<TextMeshPro>();
        damagetext.text = damage.ToString();
        
        SetDamageTextProperties();
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
    
    // flipX 상태에 따라 데미지 텍스트 애니메이션 방향 결정
    // 오른쪽 : (-1, 0.5, 0) -> (-2, -0.5, 0)
    // 왼쪽 : (1, 0.5, 0) -> (2, -0.5, 0)
    public void AnimateDamageText(bool isFacingRight)
    {
        float directionMultiplier = isFacingRight ? -1f : 1f; // 오른쪽을 바라보면 -1, 왼쪽이면 1

        Vector3 startPosition = transform.localPosition;
        Vector3 peakPoint = startPosition + new Vector3(1 * directionMultiplier, 0.5f, 0);
        Vector3 endPoint = startPosition + new Vector3(2 * directionMultiplier, -0.5f, 0);

        // 애니메이션 실행
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMove(peakPoint, 0.3f).SetEase(Ease.OutQuad))
            .Append(transform.DOLocalMove(endPoint, 0.3f).SetEase(Ease.InQuad));
        sequence.Join(damagetext.DOFade(0, 1f)); // Fade-out 
        sequence.OnComplete(() => Destroy(gameObject, 1.0f)); // 애니메이션 종료 후 객체 파괴
    }
}
