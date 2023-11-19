using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slime : MonoBehaviour
{
    // 변수
    public int Current_Hp; // 현재 체력

    // 상수
    public int HP = 100; // 체력
    public int Attack = 10; // 공격력

    public TextMeshProUGUI _HP;
    public TextMeshProUGUI _cur_HP;

    void Start()
    {
        _HP.text = HP.ToString();
        _cur_HP.text = Current_Hp.ToString();
    }
}
