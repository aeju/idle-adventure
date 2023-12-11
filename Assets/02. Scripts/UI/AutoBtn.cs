using UnityEngine;
using DG.Tweening;
using UniRx;
using UnityEngine.UI;

// 상태 : AutoOn, AutoOff
// 터치 : AutoOff -> AutoOn:회전 -> AutoOff
public class AutoBtn : MonoBehaviour
{
    public GameObject AutoOn; // Assign in inspector
    public GameObject AutoOff; // Assign in inspector

    // 현재 회전 상태
    private BoolReactiveProperty isRotating = new BoolReactiveProperty(false); // 상태 변경 -> 모든 구독자에게 알림 
    private Tween rotationTween;

    void Start()
    {
        Button AutoBtn = GetComponent<Button>(); 
        AutoBtn.OnClickAsObservable().Subscribe(_ =>
        {
            Rotation(); // 클릭하면 Rotation 실행
        }).AddTo(this);
        
        // 초기 상태 : Off 활성화
        AutoOn.SetActive(false);
        AutoOff.SetActive(true);
        
        // rotation tween 초기화
        RectTransform rectTransform = GetComponent<RectTransform>();
        rotationTween = rectTransform.DORotate(new Vector3(0, 0, 360), 2, RotateMode.FastBeyond360) // z축 360도 회전, 지속 시간 2초, 360도 이상회전 = 계속 회전
            .SetEase(Ease.Linear) // 속도 : 일정
            .SetLoops(-1, LoopType.Incremental) // 무한 반복
            .Pause(); // 시작 : 실행 x
    }

    public void Rotation()
    {
        isRotating.Value = !isRotating.Value; // 회전 상태 전환
        
        isRotating.Subscribe(rotating =>
        {
            if (rotating)
            {
                // 회전 시작 
                rotationTween.Play();
                AutoOn.SetActive(true);
                AutoOff.SetActive(false);
            }
            else
            {
                rotationTween.Pause();
                GetComponent<RectTransform>().localRotation = Quaternion.identity; // Off 상태가 됐을 때, 회전하던 이미지 초기화
                AutoOn.SetActive(false);
                AutoOff.SetActive(true);
            }
        });
    }
}