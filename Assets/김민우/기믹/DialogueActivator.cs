using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private GameObject Bang; // 느낌표 오브젝트
    [SerializeField] private GameObject dialogueObject; // 대화 UI 오브젝트
    [SerializeField] private Transform player;          // 주인공 Transform
    private bool dialogueStarted = false;              // 대화 시작 여부 체크
    private bool dialogueFinished = false;             // 대화 종료 여부 체크

    void Update()
    {
        if (player != null)
        {
            // 주인공의 x 좌표가 -8과 -7 사이일 때
            if (player.position.x >= -8f && player.position.x <= -7f && !dialogueStarted)
            {
                dialogueObject.SetActive(true); // 대화 UI 활성화
                dialogueStarted = true;        // 대화 시작 상태로 변경
                dialogueFinished = false;     // 대화 종료 상태 초기화
                Bang.SetActive(false);
                Debug.Log("Dialogue Object Activated!");
            }

            // 대화가 끝났을 때 비활성화
            if (dialogueStarted && dialogueFinished)
            {
                dialogueObject.SetActive(false); // 대화 UI 비활성화
                dialogueStarted = false;        // 대화 상태 초기화
                Debug.Log("Dialogue Object Deactivated!");
            }
        }
    }

    // 대화 종료를 외부에서 호출하기 위한 메서드
    public void EndDialogue()
    {
        dialogueFinished = true; // 대화 종료 상태 설정
    }
}
