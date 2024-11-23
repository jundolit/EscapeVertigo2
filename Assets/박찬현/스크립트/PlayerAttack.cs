using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
  [SerializeField] private float attackCooldown;

    private Animator anim;
    private PlayerMove PlayerMove;
    private float cooldownTimer = Mathf.Infinity;

   

    private void Awake()
    {
        anim = GetComponent<Animator>();
        PlayerMove = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        // 왼마우스 키를 눌렀을 때 공격
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && PlayerMove.canAttack())
            Attack();

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        anim.SetTrigger("attack");

        cooldownTimer = 2;

        // 공격 애니메이션 끝나면 트리거 리셋
        StartCoroutine(ResetAttackTrigger());
    }

    private IEnumerator ResetAttackTrigger()
    {
        // 애니메이션 길이만큼 대기
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.ResetTrigger("attack");
    }

}
