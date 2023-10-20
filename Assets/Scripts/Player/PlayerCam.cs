using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 위치를 따라가는 카메라
public class PlayerCam : MonoBehaviour
{
    public Transform playerTransform; // 플레이어 위치
    public Vector3 offset; // 플레이어 - 카메라 조절하기 위한 오프셋

    private void FixedUpdate()
    {
        // 플레이어 위치와 오프셋을 더하여 카메라의 위치를 업데이트 
        transform.position = playerTransform.position + offset;
    }
}
