using UnityEngine;

public class QuizGimmick : MonoBehaviour
{
    [SerializeField] private GameObject questionUI;      // 퀴즈 문제 UI
    [SerializeField] private GameObject bloodyBackground; // 피로 물든 배경
    [SerializeField] private GameObject safe;            // 금고 오브젝트
    [SerializeField] private GameObject[] portraits;     // 초상화 오브젝트 배열
    [SerializeField] private GameObject[] dialogueObjects; // 초상화에 대응되는 대사 UI 오브젝트
    [SerializeField] private float detectionRange = 5f;  // 플레이어 감지 거리
    [SerializeField] private GameObject runUI;           // "Run" UI
    [SerializeField] private GameObject enemy;           // 적 오브젝트
    [SerializeField] private AudioClip footstepSound;    // 발걸음 소리
    private AudioSource audioSource;                    // 오디오 소스
    private int[] interactionCounts;                    // 각 초상화의 상호작용 횟수 (초기값 0)
    public int correctAnswerIndex = 1;                  // 정답 초상화 인덱스
    private bool isPlayingFootstepSound = false;


    private bool quizStarted = false; // 퀴즈 시작 여부

    void Start()
    { 
        
        // 초기 상태 설정
        interactionCounts = new int[portraits.Length];
        for (int i = 0; i < interactionCounts.Length; i++)
        {
            interactionCounts[i] = 0; // 각 초상화 상호작용 횟수 0으로 초기화
        }
        // 초기 상태에서 UI, 배경, 금고, Run UI 및 적 비활성화
        questionUI.SetActive(false);
        bloodyBackground.SetActive(false);
        safe.SetActive(false);
        runUI.SetActive(false);
        enemy.SetActive(false);

        // AudioSource 컴포넌트 초기화
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource 컴포넌트가 필요합니다. 오브젝트에 추가하세요!");
        }
    }

    void Update()
    {
        Vector2 direction = Vector2.right; // 발사 방향 (필요 시 수정)

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
    public void InteractWithPortrait(int portraitIndex)
    {
        if (portraitIndex < 0 || portraitIndex >= portraits.Length)
        {
            Debug.LogError("잘못된 초상화 인덱스입니다!");
            return;
        }

        int currentInteraction = interactionCounts[portraitIndex];

        Debug.Log($"초상화 {portraitIndex}의 현재 상호작용 횟수: {currentInteraction}");

        if (currentInteraction == 0)
        {
            // 첫 번째 상호작용: 대사 UI 활성화
            Debug.Log($"첫 번째 상호작용: 초상화 {portraitIndex}");
            interactionCounts[portraitIndex]++;
            for (int i = 0; i < dialogueObjects.Length; i++)
            {
                if (dialogueObjects[i] != null)
                    dialogueObjects[i].SetActive(i == portraitIndex); // 선택된 초상화만 활성화
            }
        }
        else if (currentInteraction == 1)
        {
            // 두 번째 상호작용: 대사 UI 비활성화
            Debug.Log($"두 번째 상호작용: 초상화 {portraitIndex}");
            interactionCounts[portraitIndex]++;
            for (int i = 0; i < dialogueObjects.Length; i++)
            {
                if (dialogueObjects[i] != null)
                    dialogueObjects[i].SetActive(false); // 모든 대사 UI 비활성화
            }
        }
        else if (currentInteraction == 2)
        {
            // 세 번째 상호작용: 정답 여부 확인
            Debug.Log($"세 번째 상호작용: 초상화 {portraitIndex}");
            interactionCounts[portraitIndex]++; // 완료 상태로 설정
            HandleAnswer(portraitIndex);
        }
        else
        {
            Debug.Log($"초상화 {portraitIndex}와의 상호작용이 이미 완료되었습니다.");
        }
    }

    void StartQuiz()
    {
        questionUI.SetActive(true); // 퀴즈 UI 활성화
        Invoke(nameof(DeactivateQuestionUI), 3f);

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
            ActivateRunUIAndPlayFootstepSound(); // Run UI 활성화 및 발걸음 소리 재생
            Invoke(nameof(ActivateEnemy), 3f);   // 3초 후 적 활성화
        }

        EndQuiz();
    }

    void ActivateRunUIAndPlayFootstepSound()
    {
        // Run UI 활성화
        if (runUI != null)
        {
            runUI.SetActive(true);
            Debug.Log("Run UI가 활성화되었습니다.");
        }
        else
        {
            Debug.LogError("Run UI가 할당되지 않았습니다!");
        }

        // 발걸음 소리 재생
        if (audioSource != null && footstepSound != null && !isPlayingFootstepSound)
        {
            audioSource.clip = footstepSound;
            audioSource.loop = true; // 발걸음 소리를 반복 재생
            audioSource.Play();
            isPlayingFootstepSound = true;
            Debug.Log("발걸음 소리를 재생합니다.");
        }
        else if (footstepSound == null)
        {
            Debug.LogError("발걸음 소리가 할당되지 않았습니다!");
        }
    }

    void ActivateEnemy()
    {
        // 발걸음 소리 정지
        if (audioSource != null && isPlayingFootstepSound)
        {
            audioSource.Stop();
            isPlayingFootstepSound = false;
            Debug.Log("발걸음 소리를 정지합니다.");
        }

        // 적 활성화
        if (enemy != null)
        {
            enemy.SetActive(true);
            Debug.Log("적이 활성화되었습니다!");
        }
        else
        {
            Debug.LogError("적 오브젝트가 할당되지 않았습니다!");
        }
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
    void DeactivateQuestionUI()
    {
        if (questionUI != null)
        {
            questionUI.SetActive(false);
            Debug.Log("Question UI deactivated.");
        }
        else
        {
            Debug.LogError("Question UI가 할당되지 않았습니다!");
        }
    }
}
