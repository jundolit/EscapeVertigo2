using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoldierHallMove : MonoBehaviour
{
    public PlayerMove playerMove; // 플레이어 움직임 스크립트
    public float speed = 2.0f; // Soldier의 이동 속도
    public float warningDistance = 2.0f; // 플레이어와 Soldier 간의 경고 거리
    public Transform player; // 플레이어 오브젝트
    public TypingEffect soldierTypingEffect; // Soldier의 타이핑 이펙트
    public Image gameOverImg; // Canvas에 있는 GameOver 이미지 오브젝트
    public float blinkDuration = 0.5f; // 깜빡이는 간격 (초)
    public int blinkCount = 3; // 깜빡이는 횟수
    public string gameOverSceneName = "GameOver"; // 이동할 GameOver 씬 이름
    public LayerMask playerLayer; // 플레이어 레이어
    public AudioSource audioSource; // AudioSource 컴포넌트
    public AudioClip blinkSound; // 블링크 시 재생할 오디오 클립
    public Animator playerAnimator; // 플레이어 애니메이터
    public int qustID;

    private bool isWarningTriggered = false; // 경고 이벤트 발생 여부
    private bool isTypingStarted = false; // 타이핑이 시작되었는지 확인하는 플래그
    private bool dialoguesFinished = false; // 대사가 모두 끝났는지 여부
    private bool isBlinking = false; // 깜빡임이 한 번만 실행되도록 관리하는 플래그
    private Rigidbody2D soldierRigidbody;
    private PlayerMove playerMoveScript; // 플레이어의 움직임을 제어할 스크립트
    private SpriteRenderer spriteRenderer; // Soldier의 스프라이트 렌더러
    void Start()
    {
        // PlayerPrefs에서 QUSTID 값을 로드
        qustID = PlayerPrefs.GetInt("QUSTID", 0);

        // QUSTID가 2 이상이면 Soldier 비활성화 (이벤트 실행 방지)
        if (qustID >= 2)
        {
            GameLoad();
            gameObject.SetActive(false); // Soldier 오브젝트 비활성화
            return;
        }// Soldier의 Rigidbody와 PlayerMove 스크립트 가져오기
        soldierRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMoveScript = player.GetComponent<PlayerMove>();
        if (gameOverImg != null)
        {
            gameOverImg.gameObject.SetActive(false); // 처음에는 이미지 비활성화
        }

        if (soldierTypingEffect == null || player == null || audioSource == null)
        {
            Debug.LogError("필수 오브젝트 또는 AudioSource가 설정되지 않았습니다!");
        }
    }

    void Update()
    {
        if (!isBlinking)
        {
            MoveSoldier();
        }

        // Soldier가 x 좌표 -90에 도달했는지 확인
        if (transform.position.x <= -110 && qustID != 2)
        {
            // QUSTID를 2로 업데이트
            qustID = 2;
            PlayerPrefs.SetInt("QUSTID", qustID);
            PlayerPrefs.Save();
            Debug.Log("Soldier가 -110에 도달했으므로 QUSTID가 2로 설정되었습니다.");
        }
        // 플레이어와의 상호작용 감지
        CheckPlayerProximity();
    }

    // Soldier의 이동 처리
    private void MoveSoldier()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    // Raycast를 사용하여 플레이어와의 거리를 감지
    private void CheckPlayerProximity()
    {
        // Raycast를 Soldier가 이동하는 방향으로 발사
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, warningDistance, playerLayer);

        Debug.DrawRay(transform.position, Vector2.right * warningDistance, Color.red); // Raycast 시각화

        // Raycast가 플레이어와 충돌했는지 확인
        if (hit.collider != null && hit.collider.transform == player)
        {
            if (!isWarningTriggered)
            {
                OnPlayerSoldierCollision(); // 충돌 이벤트 발생
            }
        }
        else
        {
            // 플레이어가 경고 거리에서 벗어나면 상태 초기화
            isWarningTriggered = false;
        }
    }

    // 플레이어와 Soldier의 충돌 처리
    public void OnPlayerSoldierCollision()
    {
        Debug.Log("Player와 Soldier 충돌!");

        if (!dialoguesFinished) // 대사가 끝나지 않았을 경우
        {
            if (!isTypingStarted)
            {
                // Soldier의 말풍선 활성화 및 타이핑 이펙트 시작
                soldierTypingEffect.SetChildrenActive(true);
                soldierTypingEffect.StartTyping(); // 타이핑 시작

                isTypingStarted = true; // 대사 시작 플래그 설정
                StartCoroutine(WaitForTypingCompletion()); // 타이핑 완료 대기
            }

            // 경고가 발생했음을 기록하여 중복 실행 방지
            isWarningTriggered = true;
        }
        else
        {
            // 대사가 끝났으면 GameOver 이미지 깜빡임 (한 번만 실행)
            if (!isBlinking)
            {
                isBlinking = true;
                StartCoroutine(BlinkGameOverImage());
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            // 충돌 시 바로 GameOver 이미지 깜빡임 시작
            if (!isBlinking)
            {
                isBlinking = true;
                StartCoroutine(BlinkGameOverImage());
            }
        }
    }

    // 타이핑 이펙트가 끝날 때까지 대기하는 코루틴
    private IEnumerator WaitForTypingCompletion()
    {
        // 타이핑이 완료될 때까지 대기
        while (!soldierTypingEffect.TypingComplete)
        {
            yield return null;
        }

        // 타이핑이 완료되면 말풍선을 비활성화
        soldierTypingEffect.SetChildrenActive(false);
        isTypingStarted = false; // 타이핑 플래그 초기화

        // currentDialogueIndex가 대사의 끝을 넘어서면 대사가 끝난 것으로 처리
        if (soldierTypingEffect.currentDialogueIndex >= soldierTypingEffect.dialogues.Count)
        {
            dialoguesFinished = true; // 대사 종료 플래그 설정
        }
    }

    private IEnumerator BlinkGameOverImage()
    {
        // Soldier와 플레이어의 움직임 멈추기
        StopAllMovement();

        // Soldier의 flip 상태 해제
        spriteRenderer.flipX = false;
        player.GetComponent<Animator>().SetBool("isWalk", false);

        if (gameOverImg != null)
        {
            for (int i = 0; i < blinkCount; i++)
            {
                gameOverImg.gameObject.SetActive(true); // 이미지 활성화

                // 오디오 재생 (블링크 사운드가 있다면)
                if (audioSource != null && blinkSound != null)
                {
                    audioSource.PlayOneShot(blinkSound);
                }

                yield return new WaitForSeconds(blinkDuration); // 대기
                gameOverImg.gameObject.SetActive(false); // 이미지 비활성화
                yield return new WaitForSeconds(blinkDuration); // 대기
            }

            // 깜빡임이 끝나면 GameOver 씬으로 전환
            SceneManager.LoadScene(gameOverSceneName);
        }
        else
        {
            Debug.LogError("GameOverImg 오브젝트가 설정되지 않았습니다.");
        }
    }

    private void StopAllMovement()
    {
        // Soldier의 Rigidbody 속도 멈추기
        if (soldierRigidbody != null)
        {
            soldierRigidbody.velocity = Vector2.zero; // 속도 0으로 설정
            soldierRigidbody.isKinematic = true; // 물리 효과 비활성화
        }

        /*oldier의 애니메이터도 멈추기 (애니메이션에 의해 움직이지 않도록)
        if (soldierAnimator != null)
        {
            soldierAnimator.enabled = false;
        }*/

        // 플레이어 움직임 멈추기 (PlayerMove 스크립트가 있는지 확인 후 비활성화)
        if (playerMoveScript != null)
        {
            playerMoveScript.enabled = false;
        }

        // 플레이어의 Rigidbody 속도 멈추기
        Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector2.zero; // 플레이어 속도 0으로 설정
            playerRigidbody.isKinematic = true; // 플레이어 물리 효과 비활성화
        }
        else
        {
            Debug.LogError("Player의 Rigidbody2D가 할당되지 않았습니다.");
        }
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
