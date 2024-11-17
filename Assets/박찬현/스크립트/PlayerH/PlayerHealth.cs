using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    public Health healthUI;

    private void Start()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyMove enemy = collision.GetComponent<EnemyMove>();
        if (enemy) 
        {
            TakeDamage(enemy.damage);
        }
    }

    private void TakeDamage(int damage) 
    {
        currentHealth -= damage;
        healthUI.UpdateHearts(currentHealth);

        if (currentHealth <= 0) 
        {

        }
    }
}
