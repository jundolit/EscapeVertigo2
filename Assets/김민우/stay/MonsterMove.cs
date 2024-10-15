using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public Transform player;             // 플레이어를 참조
    public float chaseRange = 10f;       // 추적을 시작할 거리
    public float attackRange = 2f;       // 공격을 시작할 거리
    public float attackCooldown = 1f;    // 공격 대기 시간
    public float moveSpeed = 2f;         // 몬스터 이동 속도
    public float idleMoveTime = 3f;      // 무작위 이동 시간
    public float idleDirectionChangeInterval = 2f; // 방향 변경 간격

    private Rigidbody2D rb;
    private Animator animator;
    private float distanceToPlayer = Mathf.Infinity;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private Vector2 randomDirection;    // 무작위 방향 저장
    private float directionChangeTimer; // 방향 변경 타이머

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 참조
        animator = GetComponent<Animator>();  // Animator 컴포넌트 참조
        directionChangeTimer = idleDirectionChangeInterval;
        ChooseNewRandomDirection(); // 시작 시 무작위 방향 설정
    }

    void Update()
    {
        // 플레이어와의 거리 계산
        distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= attackRange)
        {
            // 공격 상태
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            // 추적 상태
            ChasePlayer();
        }
        else
        {
            // 무작위 이동 (Idle 상태)
            Wander();
        }

        // 공격 쿨다운 타이머 업데이트
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                isAttacking = false;
            }
        }
    }

    // 무작위 이동 로직
    void Wander()
    {
        // 일정 시간마다 방향 변경
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0f)
        {
            ChooseNewRandomDirection(); // 새로운 방향 선택
            directionChangeTimer = idleDirectionChangeInterval; // 타이머 리셋
        }

        // 설정된 무작위 방향으로 이동
        rb.velocity = randomDirection * moveSpeed;
        animator.SetBool("isMoving", true); // 이동 애니메이션 활성화
    }

    // 무작위 방향 설정
    void ChooseNewRandomDirection()
    {
        // -1에서 1 사이의 무작위 값을 생성해 방향 설정
        // -1에서 1 사이의 무작위 값을 생성해 x축 방향 설정 (y축은 고정)
        float randomX = Random.Range(-1f, 1f);
        float randomY = 0f; // y축을 고정하여 수평 방향만 움직임
        randomDirection = new Vector2(randomX, randomY);

    }

    // 추적 상태
    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
        animator.SetBool("isMoving", true); // 이동 애니메이션 활성화
    }

    // 공격 상태
    void AttackPlayer()
    {
        if (!isAttacking)
        {
            rb.velocity = Vector2.zero; // 공격 시 멈춤
            animator.SetTrigger("Attack"); // 공격 애니메이션 재생
            isAttacking = true;
            attackTimer = attackCooldown; // 공격 대기 시간 설정
        }
    }

    // 플레이어가 추적 범위 안으로 들어왔을 때 감지
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 감지되면 플레이어를 추적
            player = other.transform;
        }
    }

    // 플레이어가 추적 범위 밖으로 나갔을 때
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어를 놓침
            player = null;
            Wander();
        }
    }
}
