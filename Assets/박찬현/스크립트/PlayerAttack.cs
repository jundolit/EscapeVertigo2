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
        // �޸��콺 Ű�� ������ �� ����
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && PlayerMove.canAttack())
            Attack();

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        anim.SetTrigger("attack");

        cooldownTimer = 2;

        // ���� �ִϸ��̼� ������ Ʈ���� ����
        StartCoroutine(ResetAttackTrigger());
    }

    private IEnumerator ResetAttackTrigger()
    {
        // �ִϸ��̼� ���̸�ŭ ���
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.ResetTrigger("attack");
    }

}
