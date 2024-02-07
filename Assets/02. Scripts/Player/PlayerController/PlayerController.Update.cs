using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerController : MonoBehaviour
{
    public Slider hpSlider;
    public Slider cooldownSlider;
    
    /// <summary>
    /// 현재 플레이어 hp(%)를 hp 슬라이더의 value에 반영
    /// </summary>
    public void HPSliderUpdate()
    {
        hpSlider.value = (float) playerStats.currentHP / (float) playerStats.maxHP; 
    }
}
