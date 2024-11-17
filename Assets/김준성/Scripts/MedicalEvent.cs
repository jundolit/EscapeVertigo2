using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MedicalEvent : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public GameObject dialogueObject; // Dialogue 오브젝트 (SC)
    public GameObject guardObject; // 간수 오브젝트
    public GameObject annaObject; // 안나 오브젝트
    public Camera mainCamera; // 메인 카메라
    public SmoothCameraFollow smoothFollower; // SmoothFollower 스크립트
    public float playerTargetX = 2f; // 플레이어의 목표 X 위치
    public float moveSpeed = 2f; // 플레이어 이동 속도
    private PlayerMove playerMove; // PlayerMove 스크립트 참조
    private Animator playerAnimator; // 플레이어의 애니메이터 참조

    void Start()
    {
        // PlayerMove 스크립트와 애니메이터 가져오기
        playerMove = player.GetComponent<PlayerMove>();
        playerAnimator = player.GetComponent<Animator>();

        // PlayerMove 스크립트를 비활성화
        playerMove.enabled = false;


        // 이벤트 시퀀스 시작
        StartCoroutine(EventSequence());

    }

    private IEnumerator EventSequence()
    {
        // 1. 플레이어의 isWalk 애니메이션 활성화
        playerAnimator.SetBool("isWalk", true);

        // 플레이어를 X=2 위치로 이동
        while (player.position.x < playerTargetX)
        {
            player.position = Vector2.MoveTowards(player.position, new Vector2(playerTargetX, player.position.y), moveSpeed * Time.deltaTime);
            yield return null;
        }

        // 2. 플레이어가 멈추면 isWalk를 비활성화하고 Idle 상태로 전환
        playerAnimator.SetBool("isWalk", false);
        playerAnimator.SetBool("isIdle", true);

        // 3. 목표 위치에 도달 후 Dialogue 오브젝트 활성화
        dialogueObject.SetActive(true);
        // 간수와 안나 오브젝트를 활성화
        guardObject.SetActive(true);
        annaObject.SetActive(true);
        // 대화가 끝날 때까지 대기
        yield return new WaitUntil(() => dialogueObject.activeSelf == false);

        // 4. 카메라 설정을 원래대로 되돌리기
        mainCamera.orthographicSize = 7;

        // 5. SmoothFollower 스크립트 실행
        smoothFollower.enabled = true;

        // 6. PlayerMove 스크립트를 다시 활성화
        playerMove.enabled = true;

        // 7. QustID 값 3으로 변경
        PlayerPrefs.SetInt("QustID", 3);
        SceneManager.LoadScene("MedicHall"); // 이동할 맵 씬으로 전환

    }
}
