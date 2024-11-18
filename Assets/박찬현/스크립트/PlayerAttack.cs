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
        // Q 키를 눌렀을 때 공격
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && PlayerMove.canAttack())
            Attack();

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        // 향후 fireballs 추가 구현 가능
    }
}
