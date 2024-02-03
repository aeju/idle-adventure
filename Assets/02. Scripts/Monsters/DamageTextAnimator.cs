using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageTextAnimator : MonoBehaviour
{
    public TextMeshProUGUI damageTextPrefab;
    private Canvas canvas;

    void Awake()
    {
        canvas = FindObjectOfType<Canvas>(); // Find the Canvas in the scene.
        if (canvas == null)
        {
            Debug.LogError("Canvas not found for DamageTextAnimator.");
        }
    }

    public void ShowDamageText(int damage, Vector3 worldPosition)
    {
        try
        {
            // Existing code...
            TextMeshProUGUI damageTextInstance = Instantiate(damageTextPrefab, canvas.transform);
            //Debug.Log(damageTextInstance != null ? "Damage text instantiated" : "Failed to instantiate damage text");
            damageTextInstance.text = damage.ToString();
            
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            damageTextInstance.transform.position = screenPosition;
            
            Debug.Log(1);
            //damageTextInstance.transform.position = Camera.main.WorldToScreenPoint(position);
            Debug.Log(2);

            AnimateDamageText(damageTextInstance);
            Debug.Log(3);
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception in ShowDamageText: " + ex.Message);
        }
        // 아무 문제 x 
    }

    private void AnimateDamageText(TextMeshProUGUI damageTextInstance)
    {
        Vector3 startPosition = damageTextInstance.transform.localPosition;
        Vector3 peakPoint = startPosition + new Vector3(-1, 0.5f, 0);
        Vector3 endPoint = startPosition + new Vector3(-2, -0.5f, 0);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(damageTextInstance.transform.DOLocalMove(peakPoint, 0.3f).SetEase(Ease.OutQuad))
            .Append(damageTextInstance.transform.DOLocalMove(endPoint, 0.3f).SetEase(Ease.InQuad))
            .OnComplete(() => Destroy(damageTextInstance.gameObject));
    }
    
    /*
    public TextMeshProUGUI damageText; 

    // 데미지값으로 텍스트 초기화, 애니메이션 끝 위치 정의
    public void InitializeAndAnimate(string damageValue, Vector3 endValue)
    {
        damageText.text = damageValue;

        AnimateDamageText(endValue);
    }

    private void AnimateDamageText(Vector3 endValue)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(damageText.transform.DOMove(endValue, 1f)); // 1초 동안 endValue로 이동
        sequence.Join(damageText.DOFade(0, 1f)); // fade out (1초 동안 알파 0 되도록)
        sequence.OnComplete(() => Destroy(gameObject)); 

        sequence.Play();
    }
    */
}
