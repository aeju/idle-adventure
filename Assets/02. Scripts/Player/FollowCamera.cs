using UnityEngine;

// 기존 FollowCam 문제점 : 실행되면, 기존 카메라 위치/각도 유지 x
// 문제: target찾긴 하는데, 에러뜸! (실행순서 보장 필요) 
public class FollowCamera : MonoBehaviour
{
    public Transform target; 

    private Vector3 offset; 
    private Quaternion initialRotation; 
    
    void Start()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            // Assign the player's transform to the target
            target = playerController.transform;
        }
        else
        {
            Debug.LogError("PlayerController not found in the scene.");
        }
        
        // initial offset : 카메라 - 플레이어 위치
        offset = transform.position - target.position;
        
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // 플레이어 위치 + offset
        Vector3 newPos = target.position + offset;
        // 카메라의 위치 = newPos로 업데이트
        transform.position = newPos;
        
        transform.rotation = initialRotation;
    }
}