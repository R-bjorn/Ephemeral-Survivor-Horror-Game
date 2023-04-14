using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Player : MonoBehaviour
    {
        // Player stats
        public int health = 100;
        public int maxHealth = 200;

        void Start()
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

        void Die()
        {
            // Player death
        }

        public void Heal(int amount)
        {
            // Increase health, but not above max health
            health = Mathf.Min(health + amount, maxHealth);
        }
    }
}