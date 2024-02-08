using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handle이 매번 호출될 때마다 플레이어 컨트롤러에 대한 초기 설정 확인 : 비효율적
// -> Enter, Exit 사용 = 상태가 시작하거나 종료될 때 한 번만 필요한 작업 수행
// Enter, Handle, Exit 메소드를 강제 
public interface IPlayerState
{
    void Enter(PlayerController playerController); // 한 번만 필요한 초기화 작업 
    void Handle(PlayerController playerController); // 현재 상태가 수행해야 할 작업,행동 (상태별 로직 실행)
    void Exit(PlayerController playerController); // 상태가 종료될 때의 로직
}
