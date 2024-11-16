using UnityEngine;

public class QuizGimmick : MonoBehaviour
{
    [SerializeField] private GameObject questionUI;      // 퀴즈 문제 UI
    [SerializeField] private GameObject bloodyBackground; // 피로 물든 배경
    [SerializeField] private GameObject safe;            // 금고 오브젝트
    [SerializeField] private GameObject[] portraits;     // 초상화 오브젝트 배열
    [SerializeField] private float detectionRange = 5f;  // 플레이어 감지 거리
    [SerializeField] private LayerMask playerLayer;      // 플레이어 레이어
    public int correctAnswerIndex = 1;                   // 정답 초상화 인덱스 (0부터 시작)

    private bool quizStarted = false; // 퀴즈 시작 여부

    void Start()
    {
        // 초기 상태에서 UI, 배경, 금고 비활성화
        questionUI.SetActive(false);
        bloodyBackground.SetActive(false);
        safe.SetActive(false);
    }

    void Update()
    {
        Vector2 direction = (Vector2.right); // 발사 방향 (필요 시 수정)

        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, direction, detectionRange, LayerMask.GetMask("Player"));

        if (rayHit.collider != null)
        {
            Debug.Log($"Raycast hit: {rayHit.collider.gameObject.name}");
        }

        if (rayHit.collider != null && !quizStarted)
        {
            Debug.Log("Player detected, starting the quiz!");
            quizStarted = true;
            StartQuiz();
        }
    }


    void StartQuiz()
    {
        questionUI.SetActive(true); // 퀴즈 UI 활성화

        // 초상화에 상호작용 가능하도록 설정
        foreach (var portrait in portraits)
        {
            var trigger = portrait.GetComponent<PortraitTrigger>();
            if (trigger != null)
            {
                trigger.EnableInteraction();
            }
        }

        Debug.Log("Quiz started!");
    }

    public void HandleAnswer(int selectedIndex)
    {
        if (selectedIndex == correctAnswerIndex) // 정답인 경우
        {
            Debug.Log("정답!");
            safe.SetActive(true); // 금고 활성화
        }
        else // 오답인 경우
        {
            Debug.Log("오답!");
            bloodyBackground.SetActive(true); // 배경만 피로 물듦
        }

        EndQuiz();
    }

    void EndQuiz()
    {
        // 퀴즈 종료 후 UI 비활성화 및 상호작용 비활성화
        questionUI.SetActive(false);
        foreach (var portrait in portraits)
        {
            var trigger = portrait.GetComponent<PortraitTrigger>();
            if (trigger != null)
            {
                trigger.DisableInteraction();
            }
        }
        Debug.Log("Quiz ended.");
    }

    // Gizmos로 레이캐스트 시각화
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * detectionRange);
    }
}
