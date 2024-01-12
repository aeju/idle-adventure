using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 수정1 : Vector2 -> Vector3로 변경
// 수정2 : 플레이어 앞에서 따라가기 멈추기
// 수정3 : 타겟 -> 플레이어 태그로 찾아서 넣기
public class Monster : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;

    private bool isLive = true;

    private Rigidbody2D rigid;
    private SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        //spriter = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!isLive)
            return;
        
        // 방향(위치 차이) = 타겟 위치 - 나의 위치
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        // 플레이어의 키입력 값을 더한 이동 = 몬스터의 방향 값을 더한 이동
        rigid.MovePosition(rigid.position + nextVec);
        //rigid.velocity = Vector2.zero;
    }
    
    // 몬스터 스프라이트 방향 -> spriteRender x => 다른 방법 찾아봐야함! 
    void LateUpdate()
    {
        if (!isLive)
            return;
        // 목표의 x축 값과 자신의 x축 값을 비교하여 작으면 true가 되도록 설정
        //spriter.flipX = target.position.x < rigid.position.x;
    }


    void OnEnable() // 스크립트가 활성화 될 때, 호출되는 이벤트 함수 
    {
        //target = GameManager.instance.player.GetComponent<Rigidbody2D>();
    }
}
