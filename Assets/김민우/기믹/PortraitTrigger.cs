using UnityEngine;

public class PortraitTrigger : MonoBehaviour
{
    [SerializeField] private int portraitIndex;       // 초상화 인덱스
    [SerializeField] private float detectionRange = 2f; // 플레이어 감지 거리
    [SerializeField] private LayerMask playerLayer;   // 플레이어 레이어

    private bool interactionEnabled = false;          // 상호작용 활성화 여부

    public void EnableInteraction()
    {
        interactionEnabled = true;
    }

    public void DisableInteraction()
    {
        interactionEnabled = false;
    }

    void Update()
    {
        // 레이캐스트를 사용하여 플레이어가 초상화 아래에 있는지 확인
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, detectionRange, LayerMask.GetMask("Player"));

        if (hit.collider != null && interactionEnabled && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log($"Player interacted with portrait {portraitIndex}");

            // QuizGimmick 스크립트와 연결
            QuizGimmick quizGimmick = FindObjectOfType<QuizGimmick>();
            if (quizGimmick != null)
            {
                quizGimmick.HandleAnswer(portraitIndex);
            }
        }
    }

    void OnDrawGizmos()
    {
        // 레이캐스트 시각화 (디버깅용)
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * detectionRange);
    }
}
