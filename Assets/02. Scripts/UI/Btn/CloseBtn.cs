using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseBtn : MonoBehaviour
{
    public void CloseParentPanel()
    {
        // 버튼이 속한 부모 패널 찾기
        Transform parentPanel = transform.parent;

        // 부모 패널을 찾을 때까지 계속 상위로 이동
        while (parentPanel != null && !parentPanel.CompareTag("UIPanel"))
        {
            parentPanel = parentPanel.parent;
        }

        // 부모 패널이 발견되면 해당 패널 비활성화
        if (parentPanel != null)
        {
            parentPanel.gameObject.SetActive(false);
        }
    }
}
