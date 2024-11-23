using UnityEngine;

public class QuizGimmick : MonoBehaviour
{
    [SerializeField] private GameObject questionUI;      // ���� ���� UI
    [SerializeField] private GameObject bloodyBackground; // �Ƿ� ���� ���
    [SerializeField] private GameObject safe;            // �ݰ� ������Ʈ
    [SerializeField] private GameObject[] portraits;     // �ʻ�ȭ ������Ʈ �迭
    [SerializeField] private GameObject[] dialogueObjects; // �ʻ�ȭ�� �����Ǵ� ��� UI ������Ʈ
    [SerializeField] private float detectionRange = 5f;  // �÷��̾� ���� �Ÿ�
    [SerializeField] private GameObject runUI;           // "Run" UI
    [SerializeField] private GameObject enemy;           // �� ������Ʈ
    [SerializeField] private AudioClip footstepSound;    // �߰��� �Ҹ�
    private AudioSource audioSource;                    // ����� �ҽ�
    private int[] interactionCounts;                    // �� �ʻ�ȭ�� ��ȣ�ۿ� Ƚ�� (�ʱⰪ 0)
    public int correctAnswerIndex = 1;                  // ���� �ʻ�ȭ �ε���
    private bool isPlayingFootstepSound = false;


    private bool quizStarted = false; // ���� ���� ����

    void Start()
    { 
        
        // �ʱ� ���� ����
        interactionCounts = new int[portraits.Length];
        for (int i = 0; i < interactionCounts.Length; i++)
        {
            interactionCounts[i] = 0; // �� �ʻ�ȭ ��ȣ�ۿ� Ƚ�� 0���� �ʱ�ȭ
        }
        // �ʱ� ���¿��� UI, ���, �ݰ�, Run UI �� �� ��Ȱ��ȭ
        questionUI.SetActive(false);
        bloodyBackground.SetActive(false);
        safe.SetActive(false);
        runUI.SetActive(false);
        enemy.SetActive(false);

        // AudioSource ������Ʈ �ʱ�ȭ
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource ������Ʈ�� �ʿ��մϴ�. ������Ʈ�� �߰��ϼ���!");
        }
    }

    void Update()
    {
        Vector2 direction = Vector2.right; // �߻� ���� (�ʿ� �� ����)

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
            Debug.LogError("�߸��� �ʻ�ȭ �ε����Դϴ�!");
            return;
        }

        int currentInteraction = interactionCounts[portraitIndex];

        Debug.Log($"�ʻ�ȭ {portraitIndex}�� ���� ��ȣ�ۿ� Ƚ��: {currentInteraction}");

        if (currentInteraction == 0)
        {
            // ù ��° ��ȣ�ۿ�: ��� UI Ȱ��ȭ
            Debug.Log($"ù ��° ��ȣ�ۿ�: �ʻ�ȭ {portraitIndex}");
            interactionCounts[portraitIndex]++;
            for (int i = 0; i < dialogueObjects.Length; i++)
            {
                if (dialogueObjects[i] != null)
                    dialogueObjects[i].SetActive(i == portraitIndex); // ���õ� �ʻ�ȭ�� Ȱ��ȭ
            }
        }
        else if (currentInteraction == 1)
        {
            // �� ��° ��ȣ�ۿ�: ��� UI ��Ȱ��ȭ
            Debug.Log($"�� ��° ��ȣ�ۿ�: �ʻ�ȭ {portraitIndex}");
            interactionCounts[portraitIndex]++;
            for (int i = 0; i < dialogueObjects.Length; i++)
            {
                if (dialogueObjects[i] != null)
                    dialogueObjects[i].SetActive(false); // ��� ��� UI ��Ȱ��ȭ
            }
        }
        else if (currentInteraction == 2)
        {
            // �� ��° ��ȣ�ۿ�: ���� ���� Ȯ��
            Debug.Log($"�� ��° ��ȣ�ۿ�: �ʻ�ȭ {portraitIndex}");
            interactionCounts[portraitIndex]++; // �Ϸ� ���·� ����
            HandleAnswer(portraitIndex);
        }
        else
        {
            Debug.Log($"�ʻ�ȭ {portraitIndex}���� ��ȣ�ۿ��� �̹� �Ϸ�Ǿ����ϴ�.");
        }
    }

    void StartQuiz()
    {
        questionUI.SetActive(true); // ���� UI Ȱ��ȭ
        Invoke(nameof(DeactivateQuestionUI), 3f);

        // �ʻ�ȭ�� ��ȣ�ۿ� �����ϵ��� ����
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
        if (selectedIndex == correctAnswerIndex) // ������ ���
        {
            Debug.Log("����!");
            safe.SetActive(true); // �ݰ� Ȱ��ȭ
        }
        else // ������ ���
        {
            Debug.Log("����!");
            bloodyBackground.SetActive(true);
            ActivateRunUIAndPlayFootstepSound(); // Run UI Ȱ��ȭ �� �߰��� �Ҹ� ���
            Invoke(nameof(ActivateEnemy), 3f);   // 3�� �� �� Ȱ��ȭ
        }

        EndQuiz();
    }

    void ActivateRunUIAndPlayFootstepSound()
    {
        // Run UI Ȱ��ȭ
        if (runUI != null)
        {
            runUI.SetActive(true);
            Debug.Log("Run UI�� Ȱ��ȭ�Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("Run UI�� �Ҵ���� �ʾҽ��ϴ�!");
        }

        // �߰��� �Ҹ� ���
        if (audioSource != null && footstepSound != null && !isPlayingFootstepSound)
        {
            audioSource.clip = footstepSound;
            audioSource.loop = true; // �߰��� �Ҹ��� �ݺ� ���
            audioSource.Play();
            isPlayingFootstepSound = true;
            Debug.Log("�߰��� �Ҹ��� ����մϴ�.");
        }
        else if (footstepSound == null)
        {
            Debug.LogError("�߰��� �Ҹ��� �Ҵ���� �ʾҽ��ϴ�!");
        }
    }

    void ActivateEnemy()
    {
        // �߰��� �Ҹ� ����
        if (audioSource != null && isPlayingFootstepSound)
        {
            audioSource.Stop();
            isPlayingFootstepSound = false;
            Debug.Log("�߰��� �Ҹ��� �����մϴ�.");
        }

        // �� Ȱ��ȭ
        if (enemy != null)
        {
            enemy.SetActive(true);
            Debug.Log("���� Ȱ��ȭ�Ǿ����ϴ�!");
        }
        else
        {
            Debug.LogError("�� ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�!");
        }
    }

    void EndQuiz()
    {
        // ���� ���� �� UI ��Ȱ��ȭ �� ��ȣ�ۿ� ��Ȱ��ȭ
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

    // Gizmos�� ����ĳ��Ʈ �ð�ȭ
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
            Debug.LogError("Question UI�� �Ҵ���� �ʾҽ��ϴ�!");
        }
    }
}
