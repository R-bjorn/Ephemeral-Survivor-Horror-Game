using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Player stats
    public int health = 100;
    public int maxHealth = 200;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        // Reduce health
        health -= damage;

        // Check if player is dead
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // Player death
        throw new System.NotImplementedException();
    }

    public void Heal(int amount)
    {
        Debug.Log(health);
        // Increase health, but not above max health
        health = Mathf.Min(health + amount, maxHealth);
        Debug.Log(health);
    }
}
