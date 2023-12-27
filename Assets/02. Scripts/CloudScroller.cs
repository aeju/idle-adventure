using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 구름 레이어 : 6개
// 오른쪽으로 스크롤
// 가장 오른쪽 레이어(레이어6, 마지막 레이어) -> 레이어 width만큼 이동
// -> 첫번째 레이어(가장 왼쪽)로 이동 (좌표: 첫번째 레이어 - width)
// 구름 y축으로 움직임 추가 -> 사인파 사용 
public class CloudScroller : MonoBehaviour
{
    public float scrollSpeed = 0.5f; // 스크롤 속도
    private float originalLastPositionX; // 마지막 레이어의 원래 x좌표
    public float layerSpacing = 38.0f; // 레이어 간 간격 (레이어 너비)
    
    public float verticalAmplitude = 0.01f; // 수직 이동의 진폭
    public float verticalFrequency = 1.0f; // 수직 이동의 주파수

    void Start()
    {
        // 마지막 레이어의 원래 x좌표 저장 
        Transform lastChild = transform.GetChild(transform.childCount - 1); 
        originalLastPositionX = lastChild.position.x;
    }

    void Update()
    {
        Transform firstChild = transform.GetChild(0); // 첫 번째 자식 레이어 
        Transform lastChild = transform.GetChild(transform.childCount - 1); // 마지막 자식 레이어 

        // 각 구름 레이어를 이동 
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform cloud = transform.GetChild(i);
            cloud.position += Vector3.right * scrollSpeed * Time.deltaTime; // 구름 위치 = 오른쪽 * 스크롤 속도 * 시간 
            
            // 수직 이동 -> 사인파 사용
            float sineValue = Mathf.Sin(Time.time * verticalFrequency + i) * verticalAmplitude; // i = 위상 오프셋 
            cloud.position = new Vector3(cloud.position.x, cloud.position.y + sineValue, cloud.position.z);
            
            // 마지막 레이어 구름이 원래 위치에서 레이어 너비만큼 움직였는지 확인 
            if (lastChild.position.x >= originalLastPositionX + layerSpacing)
            {
                // 새 위치 계산 
                float newPosition = firstChild.position.x - layerSpacing;
                
                // 마지막 구름을 새 위치로 이동
                lastChild.position = new Vector3(newPosition, lastChild.position.y, lastChild.position.z);
                // 마지막 구름을 첫 번째 자식으로 설정 
                lastChild.SetAsFirstSibling();
            }
        }
    }
}
