using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class LockTime : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private float elapsedTime = 0.0f;
    private bool isCounting = false;

    public TextMeshProUGUI timerText;  // TextMeshPro UI 컴포넌트에 대한 참조

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (canvasGroup.blocksRaycasts && !isCounting)
        {
            // Start counting
            isCounting = true;
        }

        if (isCounting)
        {
            elapsedTime += Time.deltaTime;

            // Display the elapsed time using TextMeshPro
            timerText.text = Mathf.FloorToInt(elapsedTime) + " seconds have passed.";
        }
    }

    // If you want to reset the timer at any point, you can call this function.
    public void ResetTimer()
    {
        elapsedTime = 0.0f;
        isCounting = false;
    }
}
