using System.Collections;
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
    private int questID;

    void Start()
    {
        GameLoad();
        // QUSTID 값을 로드 (기본값은 0)
        questID = PlayerPrefs.GetInt("QUSTID", 0);

        if (questID == 1)
        {
            Debug.Log("QUSTID가 1이므로 GameDirector가 실행되지 않음.");
            gameObject.SetActive(false); // GameDirector를 비활성화

            return; // 이벤트 실행 없이 종료
        }

        // QUSTID가 0이면 이벤트를 시작
        StartEvent();
    }

    void StartEvent()
    {
        // 타이핑 이벤트 시작
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
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    typingEffect.SetChildrenActive(false); // 말풍선 비활성화
                    playerMove.enabled = true; // 모든 대사가 종료되면 플레이어 이동 활성화
                    Debug.Log("모든 대사가 종료되었습니다. Space 입력 후 플레이어 이동 활성화");
                    typingEffect.TypingComplete = false; // TypingComplete 초기화
                    isTypingStarted = false; // 다음 대사를 위해 타이핑 시작 플래그 초기화
                }
            }

            // 솔져의 대사 시작 조건
            if (!soldierisTypingStarted && soldierTypingEffect.currentDialogueIndex < soldierTypingEffect.dialogues.Count)
            {
                soldierisTypingStarted = true;
                soldierTypingEffect.StartTyping(); // 솔져 대사 타이핑 시작
            }

            // 모든 솔져 대사가 끝났는지 확인
            if (soldierTypingEffect.currentDialogueIndex >= soldierTypingEffect.dialogues.Count && soldierTypingEffect.TypingComplete)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    soldierTypingEffect.SetChildrenActive(false); // 솔져 말풍선 비활성화
                    soldierTypingEffect.TypingComplete = false; // 솔져 타이핑 완료 플래그 초기화
                    soldierisTypingStarted = false; // 다음 대사를 위해 플래그 초기화
                    StartCoroutine(WaitForSoldierDialogue());
                }
            }
        }
    }

    public void StartSoldierMovement()
    {
        if (!isSoldierMoving)
        {
            isSoldierMoving = true; // Soldier가 이동을 시작했음을 기록
            soldierMove.StartMoving(); // Soldier의 움직임 시작
        }
    }

    private IEnumerator WaitForSoldierDialogue()
    {
        while (soldierTypingEffect.isTyping) // 대사가 진행 중이면 기다림
        {
            yield return null; // 다음 프레임 대기
        }

        // Soldier를 이동시키고 사라지게 하는 로직
        yield return StartCoroutine(soldierMove.MoveAndDisappear()); // Soldier 이동 및 사라짐 처리
        playerMove.enabled = true; // 모든 대사가 종료되면 플레이어 이동 활성화

        // 이벤트가 끝나면 QUSTID를 1로 설정하여 다음 번에 반복되지 않도록 함
        questID = 1;
        PlayerPrefs.SetInt("QUSTID", questID);
        PlayerPrefs.Save();

        Debug.Log("대화가 끝났고, QUSTID가 1로 설정되었습니다. 플레이어 이동이 활성화되었습니다.");
    }

    // 플레이어와 Soldier의 충돌 처리
    public void OnPlayerSoldierCollision()
    {
        Debug.Log("Player와 Soldier 충돌!");
        soldierTypingEffect.SetChildrenActive(true); // Soldier의 말풍선 활성화
        soldierisTypingStarted = false; // 대사 타이핑 시작 플래그 초기화 (대사 시작 가능)
    }
    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("PlayerPosX")) return;

        float x = PlayerPrefs.GetFloat("PlayerPosX");
        float y = PlayerPrefs.GetFloat("PlayerPosY");
        float z = PlayerPrefs.GetFloat("PlayerPosZ");

        player.transform.position = new Vector3(x, y, z);
        Debug.Log("불러오기 완료: 플레이어 위치 = (" + x + ", " + y + ", " + z + ")");
    }
}
