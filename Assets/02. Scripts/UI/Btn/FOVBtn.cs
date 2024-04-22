using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

// 시야 : 세 가지 상태 존재
// 기능 : 클릭 -> fov변화 
// UI : 카메라 상태가 선택되면, 해당하는 동그라미 On GameObject 켜주기
public class FOVBtn : MonoBehaviour
{
    [SerializeField] private Camera playerCam;
    [SerializeField] private Button fovBtn;
    
    private int currentFovState = 0; 
    private readonly int[] fovStates = { 35, 45, 25 };
    
    [SerializeField] private float fovChangeSpeed = 5f; // FOV 변화 속도 조절
    [SerializeField] private float currentFOV; // 현재 시야각
    [SerializeField] private float nextFOV; // 다음 시야각
    
    
    [Header("# UI (2 - 3 - 1)")]
    [SerializeField] private GameObject[] onIndicators; // On 상태 (흰색)
    [SerializeField] private GameObject[] offIndicators; // Off 상태 (회색)
    
    void Start()
    {
        fovBtn.OnClickAsObservable().Subscribe(_ =>
        {
            ChangeFOV(); // currentFOV = nextFOV로 변경
        }).AddTo(this);
        
        // 초기 시야각 설정
        currentFOV = fovStates[currentFovState]; 
        playerCam.fieldOfView = currentFOV; 
        
        // 다음 시야각 (fovStates[currentFovState + 1])
        nextFOV = GetNextFOV(); 
        
        UpdateUI(currentFovState); // UI 초기화
    }
    
    void Update()
    {
        // currentFOV가 변하면
        if (Mathf.Abs(playerCam.fieldOfView - currentFOV) > 0.1f)
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, currentFOV, fovChangeSpeed * Time.deltaTime);
        }
    }

    private void ChangeFOV()
    {
        currentFOV = nextFOV; // 현재 시야각을 다음 시야각으로 업데이트
        currentFovState = (currentFovState + 1) % fovStates.Length; // 다음 FOV로 전환
        nextFOV = GetNextFOV(); // 다음 시야각 미리 계산 
        
        UpdateUI(currentFovState); // UI 업데이트
    }
    
    private float GetNextFOV()
    {
        return fovStates[(currentFovState + 1) % fovStates.Length]; // 다음 FOV 계산
    }

    private void UpdateUI(int fovState)
    {
        for (int i = 0; i < onIndicators.Length; i++)
        {
            onIndicators[i].SetActive(i == fovState); // on : 현재 상태만 활성화
            offIndicators[i].SetActive(i != fovState); // off : 현재 상태 제외 활성화
        }
    }
}
