using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBlink : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI blinkText; 
    [SerializeField] private float blinkSpeed = 0.5f;

    private void Start()
    {
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            blinkText.enabled = !blinkText.enabled;
            yield return new WaitForSeconds(blinkSpeed);
        }
    }
}
