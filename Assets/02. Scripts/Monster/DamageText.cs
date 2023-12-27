using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

// 생성되고 일정 시간 뒤에 파괴되도록 
public class DamageText : MonoBehaviour
{
    public float moveSpeed; // 텍스트 이동속도 
    public float alphaSpeed; // 투명도 변환속도 
    public float destroyTime;
    
    private TextMeshPro damagetext;
    
    private Color alpha;
    public int damage;
    
    void Start()
    {
        damagetext = GetComponent<TextMeshPro>();
        damagetext.text = damage.ToString();
        alpha = damagetext.color;
        //Invoke("DestroyObject", destroyTime);
        AnimateDamageText();
    }
    
    void Update()
    {
        //transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        damagetext.color = alpha;
    }

    /*
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
    */

    public void AnimateDamageText()
    {
        Vector3 startPosition = damageText.transform.localPosition;
        Vector3 peakPoint = startPosition + new Vector3(-1, 0.5f, 0);
        Vector3 endPoint = startPosition + new Vector3(-2, -0.5f, 0);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(damageText.transform.DOLocalMove(peakPoint, 0.3f).SetEase(Ease.OutQuad))
            .Append(damageText.transform.DOLocalMove(endPoint, 0.3f).SetEase(Ease.InQuad))
            .OnComplete(() => Destroy(gameObject, 1.0f)); // 1초 후 destroy
    }
    }
}
