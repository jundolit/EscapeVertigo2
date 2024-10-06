using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI speechText; // TextMeshPro 텍스트 컴포넌트
    public float typingSpeed = 0.05f; // 한 글자씩 나오는 시간 간격
    public List<string> dialogues; // 대사 목록
    public GameObject imageObject; // Image 오브젝트
    public GameObject textObject; // Text (TMP) 오브젝트
    public int currentDialogueIndex = 0; // 현재 대사 인덱스
    public bool TypingComplete { get; set; } // 타이핑 완료 여부 확인 플래그 추가
    public bool isTyping = false; // 현재 타이핑 중인지 확인 (public으로 변경하여 외부에서 접근 가능하게 함)

    void Start()
    {
        Debug.Log("TypingEffect Start called.");
        SetChildrenActive(false); // 초기 상태에서 Image와 Text (TMP) 비활성화
    }

    public void StartTyping()
    {
        Debug.Log("StartTyping called.");
        if (dialogues.Count == 0)
        {
            Debug.LogError("대사 목록이 비어 있습니다!");
            return;
        }

        // 이미 타이핑 중이라면 새로운 타이핑을 시작하지 않음
        if (!isTyping && currentDialogueIndex < dialogues.Count)
        {
            SetChildrenActive(true); // 타이핑 시작 시 Image와 Text (TMP) 활성화
            StartCoroutine(TypeText()); // 타이핑 코루틴 실행
        }
        else if (currentDialogueIndex >= dialogues.Count)
        {
            // 대사가 끝나면 UI를 비활성화하고 종료
            SetChildrenActive(false);
            Debug.Log("모든 대사가 종료되었습니다.");
        }
    }

    private IEnumerator TypeText()
    {
        // 타이핑 시작
        TypingComplete = false;

        string fullText = dialogues[currentDialogueIndex];
        speechText.text = ""; // 새로운 대사를 출력하기 전에 초기화

        foreach (char letter in fullText.ToCharArray())
        {
            speechText.text += letter;  // 한 글자씩 추가
            AdjustImageSize(); // 글자가 추가될 때마다 이미지 크기 조정
            yield return new WaitForSeconds(typingSpeed); // 지연 시간
        }

        // 타이핑 완료
        TypingComplete = true;
        Debug.Log("타이핑 완료");

        // 대사 인덱스 증가
        currentDialogueIndex++;

        // 타이핑 완료 후 Space 키 입력 대기
        while (TypingComplete)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 다음 대사로 진행
                if (currentDialogueIndex < dialogues.Count)
                {
                    // 다음 대사 타이핑 시작
                    Debug.Log("다음 대사 시작: " + currentDialogueIndex);
                    StartCoroutine(TypeText()); // 다음 대사를 타이핑
                }
                else
                {
                    // 대화가 모두 끝났을 경우에만 UI를 비활성화
                    SetChildrenActive(false);
                    Debug.Log("모든 대사가 종료되었습니다.");

                    FindObjectOfType<GameDirector>().playerMove.enabled = true; // GameDirector에 접근하여 플레이어 이동 활성화

                }
                yield break; // 코루틴 종료
            }

            yield return null; // 다음 프레임 대기
        }
    }



    private void AdjustImageSize()
    {
        Vector2 textSize = speechText.GetPreferredValues(speechText.text);
        float widthPadding = 40f; // 너비 여백
        float heightPadding = 20f; // 높이 여백
        imageObject.GetComponent<RectTransform>().sizeDelta = new Vector2(textSize.x + widthPadding, textSize.y + heightPadding); // 말풍선 여백 추가
        Debug.Log("말풍선 크기 조정 완료.");
    }

    public void SetChildrenActive(bool isActive)
    {
        imageObject.SetActive(isActive);
        textObject.SetActive(isActive);
        Debug.Log($"SetChildrenActive called with value: {isActive}");
    }
}
