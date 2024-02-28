using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBlink : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI blinkText; 
    private readonly float blinkSpeed = 0.5f;

    private void Start()
    {
        blinkText.enabled = true;
        StartCoroutine(InitialDelay());
    }
    
    private IEnumerator InitialDelay()
    {
        yield return new WaitForSeconds(blinkSpeed); // blinkSpeed만큼 대기
        StartCoroutine(Blink()); // 그 후에 깜빡임 시작
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
