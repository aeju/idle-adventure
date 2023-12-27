using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 생성되고 일정 시간 뒤에 파괴되도록 
public class DamageText : MonoBehaviour
{
    public float moveSpeed; // 텍스트 이동속도 
    public float alphaSpeed; // 투명도 변환속도 
    public float destroyTime;
    
    private TextMeshPro text;
    private Color alpha;
    public int damage;
    
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        text.text = damage.ToString();
        alpha = text.color;
        Invoke("DestroyObject", destroyTime);
    }
    
    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
