using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public Transform player; // 플레이어 오브젝트
    public TypingEffect typingEffect; // TypingEffect 스크립트
    public PlayerMove playerMove; // 플레이어 움직임 스크립트

    private bool isTypingStarted = false;

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

        // 게임 시작 시 플레이어 위치 고정
        player.position = new Vector3(0f, player.position.y, player.position.z);
        typingEffect.StartTyping();
    }

    void Update()
    {
        if (player != null && typingEffect != null && playerMove != null)
        {
            // 타이핑 중일 때는 플레이어 이동 비활성화
            if (typingEffect.isTyping)
            {
                playerMove.enabled = false; // 타이핑 중에는 플레이어 이동 비활성화
                return; // 타이핑 중일 경우 Update 종료
            }

            // 타이핑 시작 조건 (플레이어가 x >= 0 일 때)
            if (player.position.x >= 0 && !isTypingStarted && typingEffect.currentDialogueIndex < typingEffect.dialogues.Count)
            {
                isTypingStarted = true; // 타이핑 시작 여부 플래그 설정
                typingEffect.StartTyping(); // 타이핑 시작
                playerMove.enabled = false; // 타이핑 시작 시 플레이어 이동 비활성화
            }

            // 모든 대사가 끝났는지 확인
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
        }
    }





}
