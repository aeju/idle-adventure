using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageTextAnimator : MonoBehaviour
{
    public TextMeshProUGUI damageText; // Assign your TMP object here

    public void InitializeAndAnimate(string damageValue, Vector3 endValue)
    {
        damageText.text = damageValue;

        AnimateDamageText(endValue);
    }

    private void AnimateDamageText(Vector3 endValue)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(damageText.transform.DOMove(endValue, 1f)); // Move text
        sequence.Join(damageText.DOFade(0, 1f)); // Fade out text simultaneously
        sequence.OnComplete(() => Destroy(gameObject)); // Destroy the GameObject

        sequence.Play();
    }
}
