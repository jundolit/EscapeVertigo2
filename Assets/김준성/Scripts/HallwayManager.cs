using System.Collections;
using UnityEngine;

public class HallwayManager : MonoBehaviour
{
    public TypingEffect soldierTypingEffect; // Soldier의 타이핑 이펙트
    public Transform player; // 플레이어 오브젝트
    public SoldierHallMove soldierMove; // Soldier의 이동 스크립트 (왼쪽 이동)

    private bool isTypingStarted = false; // 타이핑이 시작되었는지 확인하는 플래그

    void Start()
    {
        // 씬 초기화
        InitializeScene();
    }

    // 씬 초기화
    private void InitializeScene()
    {
        // Soldier의 이동 시작
        soldierMove.enabled = true;

        // 타이핑 이펙트를 비활성화
        if (soldierTypingEffect != null)
        {
            soldierTypingEffect.SetChildrenActive(false);
        }
    }

    // 플레이어와 Soldier의 충돌 이벤트 처리
    public void OnPlayerSoldierCollision()
    {
        Debug.Log("Player와 Soldier 충돌!");

        if (!isTypingStarted && soldierTypingEffect.dialogues.Count > 0) // TypingEffect의 대사 목록을 참조
        {
            // Soldier의 말풍선 활성화 및 타이핑 이펙트 시작
            soldierTypingEffect.SetChildrenActive(true);

            // 타이핑 이펙트를 통해 대사 출력
            soldierTypingEffect.currentDialogueIndex = 0; // 대사 인덱스 초기화
            soldierTypingEffect.StartTyping(); // 타이핑 시작

            isTypingStarted = true; // 대사 시작 플래그 설정

            // 대사가 끝난 후 타이핑 이펙트 종료
            StartCoroutine(WaitForTypingCompletion());
        }
    }

    // 타이핑 이펙트가 끝날 때까지 대기하는 코루틴
    private IEnumerator WaitForTypingCompletion()
    {
        // 타이핑이 완료될 때까지 대기
        while (!soldierTypingEffect.TypingComplete)
        {
            yield return null;
        }

        // 타이핑이 완료되면 말풍선 비활성화
        soldierTypingEffect.SetChildrenActive(false);
        Debug.Log("타이핑 완료, 말풍선 비활성화.");
        isTypingStarted = false; // 대사 타이핑 플래그 초기화
    }
}
