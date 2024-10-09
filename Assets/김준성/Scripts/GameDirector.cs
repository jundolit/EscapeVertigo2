using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public Transform player; // 플레이어 오브젝트
    public TypingEffect typingEffect; // TypingEffect 스크립트
    public PlayerMove playerMove; // 플레이어 움직임 스크립트
    public SoldierMove soldierMove; // SoldierMove 스크립트 추가
    public TypingEffect soldierTypingEffect; // Soldier의 말풍선 효과 추가
    public Animator playerAnimator; // 플레이어 애니메이터

    private bool isTypingStarted = false; // 대사 타이핑 시작 여부
    private bool isSoldierMoving = false; // Soldier가 이미 이동 중인지 여부
    private bool soldierisTypingStarted = false; // Soldier 대사 타이핑 시작 여부


    void Start()
    {
        // 컴포넌트 확인
        if (player == null)
        {
            Debug.LogError("Player Transform이 할당되지 않았습니다!");
        }

        if (typingEffect == null)
        {
            Debug.LogError("TypingEffect 스크립트가 할당되지 않았습니다!");
        }

        if (playerMove == null)
        {
            Debug.LogError("PlayerMovement 스크립트가 할당되지 않았습니다!");
        }

        if (soldierTypingEffect == null)
        {
            Debug.LogError("Soldier TypingEffect 스크립트가 할당되지 않았습니다!");
        }

        // 게임 시작 시 플레이어 위치 고정
        player.position = new Vector3(0f, player.position.y, player.position.z);
        typingEffect.StartTyping();
    }

    void Update()
    {
        if (player != null && typingEffect != null && playerMove != null && soldierTypingEffect != null)
        {
            // 플레이어가 x >= 3일 때 이동 비활성화 및 솔져 이동 시작
            if (player.position.x >= 3 && !isSoldierMoving)
            {
                player.position = new Vector3(3f, player.position.y, player.position.z);
                playerMove.enabled = false; // 플레이어 이동 비활성화
                player.GetComponent<Animator>().SetBool("isWalk", false);
                // Idle 애니메이션으로 전환

                StartSoldierMovement();
            }
            else
            {
                // 타이핑 시작 조건 (플레이어가 x >= 0 일 때)
                if (!isTypingStarted && typingEffect.currentDialogueIndex < typingEffect.dialogues.Count)
                {
                    isTypingStarted = true; // 타이핑 시작 여부 플래그 설정
                    typingEffect.StartTyping(); // 플레이어 대사 타이핑 시작
                    playerMove.enabled = false; // 타이핑 시작 시 플레이어 이동 비활성화
                }
            }

            // 모든 플레이어 대사가 끝났는지 확인
            if (typingEffect.currentDialogueIndex >= typingEffect.dialogues.Count && typingEffect.TypingComplete)
            {
                // Space 키를 눌렀을 때 대화창을 닫고 플레이어 이동 활성화
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    typingEffect.SetChildrenActive(false); // 말풍선 비활성화
                    playerMove.enabled = true; // 모든 대사가 종료되면 플레이어 이동 활성화
                    Debug.Log("모든 대사가 종료되었습니다. Space 입력 후 플레이어 이동 활성화");

                    // 초기화
                    typingEffect.TypingComplete = false; // TypingComplete 초기화
                    isTypingStarted = false; // 다음 대사를 위해 타이핑 시작 플래그 초기화
                }
            }

            // 솔져의 대사 시작 조건
            if (!soldierisTypingStarted && soldierTypingEffect.currentDialogueIndex < soldierTypingEffect.dialogues.Count)
            {
                // 솔져 대사 타이핑 시작
                soldierisTypingStarted = true;
                soldierTypingEffect.StartTyping(); // 솔져 대사 타이핑 시작
            }

            // 모든 솔져 대사가 끝났는지 확인
            if (soldierTypingEffect.currentDialogueIndex >= soldierTypingEffect.dialogues.Count && soldierTypingEffect.TypingComplete)
            {
                // Space 키를 눌렀을 때 솔져 대화창을 닫고 다음 대사로 이동
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    soldierTypingEffect.SetChildrenActive(false); // 솔져 말풍선 비활성화
                    soldierTypingEffect.TypingComplete = false; // 솔져 타이핑 완료 플래그 초기화
                    soldierisTypingStarted = false; // 다음 대사를 위해 플래그 초기화

                    // 솔져가 이동을 시작하는 로직 추가
                    StartCoroutine(WaitForSoldierDialogue());
                   

                }
            }
        }
    }


    // Soldier가 움직이기 시작할 조건 (Player가 x=3 이상일 때)
    public void StartSoldierMovement()
    {
        if (!isSoldierMoving)
        {
            isSoldierMoving = true; // Soldier가 이동을 시작했음을 기록
            soldierMove.StartMoving(); // Soldier의 움직임 시작
        }
    }

    // 플레이어와 Soldier의 충돌 처리
    public void OnPlayerSoldierCollision()
    {
        Debug.Log("Player와 Soldier 충돌!");
        soldierTypingEffect.SetChildrenActive(true); // Soldier의 말풍선 활성화
        soldierisTypingStarted = false; // 대사 타이핑 시작 플래그 초기화 (대사 시작 가능)
    }

    // Soldier 대사가 끝날 때까지 대기
    private IEnumerator WaitForSoldierDialogue()
    {
        while (soldierTypingEffect.isTyping) // 대사가 진행 중이면 기다림
        {
            yield return null; // 다음 프레임 대기
        }

        // Soldier를 이동시키고 사라지게 하는 로직
        yield return StartCoroutine(soldierMove.MoveAndDisappear()); // Soldier 이동 및 사라짐 처리
        playerMove.enabled = true; // 모든 대사가 종료되면 플레이어 이동 활성화

        // 대화가 끝난 후 플레이어 이동 활성화
        Debug.Log("대화가 끝났고, 플레이어 이동이 활성화되었습니다.");
    }
}
