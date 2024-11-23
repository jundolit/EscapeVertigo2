using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMove : MonoBehaviour
{
    public Transform player;  // �÷��̾��� Transform
    public float speed = 2.0f; // Soldier �̵� �ӵ�
    private bool shouldMove = false; // Soldier�� �������� �ϴ��� ����
    private bool isMoving = false; // Soldier�� �̵� ������ ����

    private Animator anim; // Animator ������Ʈ

    void Awake()
    {
        anim = GetComponent<Animator>(); // Animator ������Ʈ ��������
    }

    // Soldier�� �̵��� ������ �� ȣ���ϴ� �޼���
    public void StartMoving()
    {
        if (!isMoving) // Soldier�� �̹� �̵� ������ Ȯ��
        {
            isMoving = true; // �̵� ���� ���
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true); // ������Ʈ�� Ȱ��ȭ
            }
            shouldMove = true; // Soldier�� �̵��ؾ� ��

            anim.SetBool("isWalk", true); // �̵� �ִϸ��̼� Ȱ��ȭ
        }
    }

    void Update()
    {
        MoveTowardsPlayer(); // �� �����Ӹ��� �÷��̾ ���� �̵�
    }

    void MoveTowardsPlayer()
    {
        // Soldier�� �÷��̾�� �浹�� ������ �÷��̾� �������� �̵�
        if (shouldMove)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Soldier�� �̵� ���̹Ƿ� isWalk�� true�� ����
            anim.SetBool("isWalk", true);
        }
        else
        {
            // �̵� ���� ��� isWalk ��Ȱ��ȭ
            anim.SetBool("isWalk", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Soldier�� �÷��̾�� �浹���� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x); // x�� �������� ������
            transform.localScale = scale;

            shouldMove = false; // �浹 �� Soldier ������ ����
            isMoving = false; // �̵� ���� ���·� ����
            anim.SetBool("isWalk", false); // �̵� �ִϸ��̼� ��Ȱ��ȭ
            Debug.Log("Player�� �浹!");

            // GameDirector�� OnPlayerSoldierCollision �޼��� ȣ��
            FindObjectOfType<GameDirector>().OnPlayerSoldierCollision();
        }
    }

    public IEnumerator MoveAndDisappear()
    {
        // SpriteRenderer ��������
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Soldier�� �̵� ������ �� Walk �ִϸ��̼� Ȱ��ȭ
        anim.SetBool("isWalk", true);

        // ������ ���������� ���� (Sprite�� Flip)
        spriteRenderer.flipX = true;

        // Soldier�� ���������� 3�ʰ� �̵�
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(11.0f, 0, 0); // ���������� 11 ���� �̵�

        while (elapsedTime < 3.0f)
        {
            // �� ������ Soldier�� �̵�
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 3.0f);

            // �̵� �� Walk �ִϸ��̼� ����
            anim.SetBool("isWalk", true);

            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        // �̵� �Ϸ� �� Walk �ִϸ��̼� ��Ȱ��ȭ
        anim.SetBool("isWalk", false);

        // Soldier ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);
    }


}
