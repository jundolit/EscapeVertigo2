using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMove : MonoBehaviour
{
    public Transform player;  // 플레이어의 Transform
    public float speed = 2.0f; // Soldier 이동 속도
    private bool shouldMove = false; // Soldier가 움직여야 하는지 여부
    private bool isMoving = false; // Soldier가 이동 중인지 여부

    // Soldier가 이동을 시작할 때 호출하는 메서드
    public void StartMoving()
    {
        if (!isMoving) // Soldier가 이미 이동 중인지 확인
        {
            isMoving = true; // 이동 시작 기록
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true); // 오브젝트를 활성화
            }
            shouldMove = true; // Soldier가 이동해야 함
        }
    }

    void Update()
    {
        MoveTowardsPlayer(); // 매 프레임마다 플레이어를 향해 이동
    }

    void MoveTowardsPlayer()
    {
        // Soldier가 플레이어와 충돌할 때까지 플레이어 방향으로 이동
        if (shouldMove)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Soldier가 플레이어와 충돌했을 때
        if (collision.gameObject.CompareTag("Player"))
        {
            shouldMove = false; // 충돌 시 Soldier 움직임 멈춤
            isMoving = false; // 이동 중지 상태로 설정
            Debug.Log("Player와 충돌!");

            // GameDirector의 OnPlayerSoldierCollision 메서드 호출
            FindObjectOfType<GameDirector>().OnPlayerSoldierCollision();
        }
    }

    // Soldier를 이동시키고 사라지게 하는 코루틴
    public IEnumerator MoveAndDisappear()
    {
        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x); // x축 방향으로 뒤집기
        transform.localScale = scale;
        // Soldier를 오른쪽으로 3초간 이동
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(9.0f, 0, 0); // 오른쪽으로 2 유닛 이동

        while (elapsedTime < 3.0f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 3.0f);
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // Soldier 오브젝트 비활성화
        gameObject.SetActive(false);
        
    }
}
