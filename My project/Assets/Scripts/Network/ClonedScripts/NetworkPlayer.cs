using TMPro;
using UnityEngine;
using Unity.Netcode;

namespace Game_Manager.Mind.Game_Scripts
{
    public class NetworkPlayer : NetworkBehaviour
    {
        // Player stats
        public int health = 100;
        public int maxHealth = 200;
        public TextMeshProUGUI healthCounter;

        // Start is called before the first frame update
        void Start()
        {
            InvokeRepeating("ReduceHealth", 1f, 1f);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void ReduceHealth()
        {
            health -= 1;
            healthCounter.text = health.ToString();
            Debug.Log(health);
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
}
