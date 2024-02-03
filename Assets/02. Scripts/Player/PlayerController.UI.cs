using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerController : MonoBehaviour
{
    public Slider hpSlider;
    public Slider cooldownSlider;
    
    public void HPSliderUpdate()
    {
        // 현재 플레이어 hp(%)를 hp 슬라이더의 value에 반영
        hpSlider.value = (float) currentHP / (float) maxHP; 
    }
}
