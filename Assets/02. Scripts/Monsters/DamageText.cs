using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

// 생성되고 일정 시간 뒤에 파괴되도록 
public class DamageText : MonoBehaviour
{
    private TextMeshPro damagetext;
    private EnemyFSM enemyFSM;
    
    public int damage;
    
    
    void Start()
    {
        damagetext = GetComponent<TextMeshPro>();
        damagetext.text = damage.ToString();
        enemyFSM = GetComponentInParent<EnemyFSM>();

        if (enemyFSM != null)
        {
            if (enemyFSM.flipX == true)
            {
                AnimateDamageTextRight();
            }
            else
            {
                AnimateDamageTextLeft();
            }
        }
    }

    public void AnimateDamageTextRight()
    {
        // 애니메이션 수정하여 방향에 따라 다르게 처리
        Vector3 startPosition = transform.localPosition;
        Vector3 peakPoint = startPosition + new Vector3(-1, 0.5f, 0); 
        Vector3 endPoint = startPosition + new Vector3(-2, -0.5f, 0);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMove(peakPoint, 0.3f).SetEase(Ease.OutQuad))
                .Append(transform.DOLocalMove(endPoint, 0.3f).SetEase(Ease.InQuad)); 
                
        sequence.Join(damagetext.DOFade(0, 1f)); // Fade-out 

        sequence.OnComplete(() => Destroy(gameObject, 1.0f)); // 1초 후 destroy
    }
    
    public void AnimateDamageTextLeft()
    {
        // 애니메이션 수정하여 방향에 따라 다르게 처리
        Vector3 startPosition = transform.localPosition;
        Vector3 peakPoint = startPosition + new Vector3(1, 0.5f, 0); 
        Vector3 endPoint = startPosition + new Vector3(2, -0.5f, 0);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMove(peakPoint, 0.3f).SetEase(Ease.OutQuad))
            .Append(transform.DOLocalMove(endPoint, 0.3f).SetEase(Ease.InQuad)); 
                
        sequence.Join(damagetext.DOFade(0, 1f)); // Fade-out 

        sequence.OnComplete(() => Destroy(gameObject, 1.0f)); // 1초 후 destroy
    }
    
    
}
