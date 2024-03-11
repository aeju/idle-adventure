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
    [SerializeField] private int currentFOV;
    
    [SerializeField] private Camera playerCam;
    [SerializeField] private Button fovBtn;

    [SerializeField] private int currentFovState = 0; 
    private readonly int[] fovStates = { 35, 45, 25 };
    
    // [UI]
    [SerializeField] private GameObject firstOn;
    [SerializeField] private GameObject firstOff;

    [SerializeField] private GameObject secondOn;
    [SerializeField] private GameObject secondOff;

    [SerializeField] private GameObject thirdOn;
    [SerializeField] private GameObject thirdOff;

    

    void Start()
    {
        fovBtn.OnClickAsObservable().Subscribe(_ =>
        {
            ChangeFOV();
        }).AddTo(this);

        playerCam.fieldOfView = fovStates[currentFovState]; // playerCam fov 초기값 설정
        currentFOV = fovStates[currentFovState]; // 인스펙터창 확인용

        UpdateUI(currentFovState); // fov 상태 확인 UI 업데이트 
    }

    private void ChangeFOV()
    {
        currentFovState++;
        if (currentFovState >= fovStates.Length)
        {
            currentFovState = 0; // currentFovState 초기화 
        }
        playerCam.fieldOfView = fovStates[currentFovState];
        currentFOV = fovStates[currentFovState];
        
        UpdateUI(currentFovState);
    }

    private void UpdateUI(int fovState)
    {
        // On 이미지 전부 꺼주기 
        firstOn.SetActive(false);
        secondOn.SetActive(false);
        thirdOn.SetActive(false); 

        // ui : 2 -> 3 -> 1
        switch (fovState)
        {
            case 0:
                secondOn.SetActive(true); // 중간On 켜주기
                firstOff.SetActive(true); // 나머지는 Off 켜주기
                thirdOff.SetActive(true);
                break;
            case 1:
                thirdOn.SetActive(true); // 3번째 On 켜주기
                firstOff.SetActive(true);
                secondOff.SetActive(true);
                break;
            case 2:
                firstOn.SetActive(true); // 1번째 On 켜주기
                secondOff.SetActive(true);
                thirdOff.SetActive(true);
                break;
        }
    }
}
