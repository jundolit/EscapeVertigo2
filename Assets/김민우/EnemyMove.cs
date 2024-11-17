using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public Transform player;  // 주인공의 Transform을 저장
    public float detectionRange = 20.0f;  // 감지 거리 설정
    public float attackAnimationSpeed = 1.5f; // 공격 애니메이션 속도 설정
    private bool isChasing = false;  // 주인공 추적 여부 체크
    private bool hasLineOfSight = false; // 시야 안에 있는지 체크
    private bool isAttacking = false; // 공격 중인지 체크
    public int damage = 1;

    [SerializeField] private AudioClip enemySound; // 적 소리 클립
    private AudioSource audioSource;             // 오디오 소스

    [Header("Game Over Settings")]
    public Image gameOverImg; // GameOver 이미지
    public string gameOverSceneName = "GameOver"; // 이동할 GameOver 씬 이름
    public int blinkCount = 3; // 깜빡임 횟수
    public float blinkDuration = 0.5f; // 깜빡임 간격
    private bool isBlinking = false; // 깜빡임 중인지 확인

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // AudioSource 초기화
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource 컴포넌트가 필요합니다. 오브젝트에 추가하세요!");
        }

        if (gameOverImg != null)
        {
            gameOverImg.gameObject.SetActive(false); // GameOver 이미지를 초기 비활성화
        }

        Invoke("Think", 5); // 초기 행동 패턴 설정
    }

    void Update()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange, LayerMask.GetMask("Player"));

        Debug.DrawRay(transform.position, directionToPlayer.normalized * detectionRange, Color.red); // 시각화

        if (rayHit.collider != null && rayHit.collider.CompareTag("Player"))
        {
            Debug.Log("Player detected!");
            isChasing = true;
            hasLineOfSight = true;
        }
        else
        {
            hasLineOfSight = false;
        }

        if (isChasing && hasLineOfSight && !isAttacking) // 플레이어 인식하고 따라가는 코드
        {
            Vector2 direction = directionToPlayer.normalized;
            rigid.velocity = new Vector2(direction.x * 7, rigid.velocity.y); // 플레이어 따라가는 속도
            anim.SetInteger("WalkSpeed", 1);
            spriteRenderer.flipX = direction.x < 0;
        }
        else if (!isAttacking)
        {
            rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            // 플레이어와 닿으면 추적 멈추고 공격 상태로 전환
            isChasing = false;
            isAttacking = true;
            rigid.velocity = Vector2.zero; // 멈추기
            anim.SetInteger("WalkSpeed", 0); // 걷기 애니메이션 정지
            anim.SetTrigger("Attack"); // 공격 애니메이션 트리거
            anim.SetFloat("AttackSpeed", attackAnimationSpeed); // 공격 애니메이션 속도 설정

            // GameOver 이미지 깜빡임은 별도 처리 (애니메이션 유지)
            if (!isBlinking)
            {
                isBlinking = true;
                StartCoroutine(BlinkGameOverImage());
            }
        }
    }


    private IEnumerator BlinkGameOverImage()
    {
        // 모든 움직임 멈추기
        StopAllMovement();

        if (gameOverImg != null)
        {
            for (int i = 0; i < blinkCount; i++)
            {
                gameOverImg.gameObject.SetActive(true); // GameOver 이미지 활성화
                yield return new WaitForSeconds(blinkDuration);
                gameOverImg.gameObject.SetActive(false); // GameOver 이미지 비활성화
                yield return new WaitForSeconds(blinkDuration);
            }

            // 깜빡임 완료 후 씬 전환
            SceneManager.LoadScene(gameOverSceneName);
        }
        else
        {
            Debug.LogError("GameOverImg가 설정되지 않았습니다.");
        }
    }

    private void StopAllMovement()
    {
    

        // 플레이어 움직임 멈추기
        if (player != null)
        {
            Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                playerRigidbody.velocity = Vector2.zero;
                playerRigidbody.isKinematic = true;
            }

            PlayerMove playerMove = player.GetComponent<PlayerMove>();
            if (playerMove != null)
            {
                playerMove.enabled = false;
            }
        }
    }

    // Animation Event에서 호출되는 함수
    public void PlayEnemySound()
    {
        if (audioSource != null && enemySound != null)
        {
            audioSource.PlayOneShot(enemySound); // 적 소리 재생
            Debug.Log("Enemy sound played!");
        }
        else if (enemySound == null)
        {
            Debug.LogError("Enemy sound clip이 설정되지 않았습니다!");
        }
    }

    void Think()
    {
        if (!isChasing && !isAttacking)  // 주인공을 추적 중이 아닐 때만 실행
        {
            nextMove = Random.Range(-1, 2);
            Invoke("Think", 5);
            anim.SetInteger("WalkSpeed", nextMove);

            if (nextMove != 0)
                spriteRenderer.flipX = nextMove == -1;
        }
    }
}
