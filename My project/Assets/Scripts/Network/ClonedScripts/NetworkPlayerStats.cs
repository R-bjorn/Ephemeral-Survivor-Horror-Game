using TMPro;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Game_Manager.Mind.Game_Scripts
{
    public class NetworkPlayerStats : NetworkBehaviour
    {
        // Player stats
        public int health = 100;
        public int maxHealth = 200;
        public TextMeshProUGUI healthCounter;

        // Start is called before the first frame update
        void Start()
        {
            GameObject healthCounterObject = GameObject.Find("Health Count");
            healthCounter = healthCounterObject.GetComponent<TextMeshProUGUI>();
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
            // Check if player is dead
            if (health <= 0)
            {
                Die();
            }
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
            Debug.Log("You Died!");
            SceneManager.LoadScene("UI");
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
