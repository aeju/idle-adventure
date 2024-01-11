using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 하나의 토글 on -> 나머지 : off
// 선택되는 UI -> 켜주기 
public class FrameRate : MonoBehaviour
{
    public Toggle toggle30;
    public Toggle toggle45;
    public Toggle toggle60;

    public GameObject defaultImage30;
    public GameObject selectedImage30;
    
    public GameObject defaultImage45;
    public GameObject selectedImage45;
    
    public GameObject defaultImage60;
    public GameObject selectedImage60;

    private void Start()
    {
        // 기본 FPS 설정
        SetFrameRate(PlayerPrefs.GetInt("FrameRate", 60));

        // 토글 이벤트 리스너 할당
        toggle30.onValueChanged.AddListener(delegate { ToggleChanged(toggle30, 30); });
        toggle45.onValueChanged.AddListener(delegate { ToggleChanged(toggle45, 45); });
        toggle60.onValueChanged.AddListener(delegate { ToggleChanged(toggle60, 60); });

        // 저장된 FPS에 따라 토글 상태 설정
        UpdateToggleStates();
    }

    void ToggleChanged(Toggle changedToggle, int frameRate)
    {
        SetFrameRate(frameRate);
        UpdateToggleStates();
    }

    void SetFrameRate(int frameRate)
    {
        // FPS 설정
        Application.targetFrameRate = frameRate;
        
        // FPS 저장
        PlayerPrefs.SetInt("FrameRate", frameRate);
        PlayerPrefs.Save();
    }

    void UpdateToggleStates()
    {
        int savedFrameRate = PlayerPrefs.GetInt("FrameRate", 60);

        // 각 토글 상태에 따라 이미지 활성화/비활성화
        defaultImage30.SetActive(savedFrameRate != 30);
        selectedImage30.SetActive(savedFrameRate == 30);
        
        defaultImage45.SetActive(savedFrameRate != 45);
        selectedImage45.SetActive(savedFrameRate == 45);
        
        defaultImage60.SetActive(savedFrameRate != 60);
        selectedImage60.SetActive(savedFrameRate == 60);
    }
}
