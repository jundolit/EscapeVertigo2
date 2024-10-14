using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.enabled = false;  // 처음에는 보이지 않음
        Invoke("Think", 5);
    }

    void Update()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange, LayerMask.GetMask("Player"));

        Debug.DrawRay(transform.position, directionToPlayer.normalized * detectionRange, Color.red); // 시각화

        if (rayHit.collider != null && rayHit.collider.CompareTag("Player"))
        {
            Debug.Log("Player detected!");
            spriteRenderer.enabled = true;
            isChasing = true;
            hasLineOfSight = true;
        }
        else
        {
            hasLineOfSight = false;
            if (!isChasing)
                spriteRenderer.enabled = false;
        }

        if (isChasing && hasLineOfSight && !isAttacking)//플레이어 인식하고 따라가는 코드
        {
            Vector2 direction = directionToPlayer.normalized;
            rigid.velocity = new Vector2(direction.x * 7, rigid.velocity.y);//플레이어 따라가는 속도 
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

            // 코루틴을 시작하여 공격 애니메이션이 완료되면 Idle로 돌아가게 함
            StartCoroutine(AttackCoroutine());
        }
    }

    IEnumerator AttackCoroutine()
    {
        // 공격 애니메이션이 완료될 때까지 대기
        yield return new WaitForSeconds(1f / attackAnimationSpeed);

        // 공격이 끝나면 대기 상태로 전환
        isAttacking = false;
        anim.ResetTrigger("Attack"); // 공격 애니메이션 트리거 초기화
        anim.SetInteger("WalkSpeed", 0); // idle 상태로 전환
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
