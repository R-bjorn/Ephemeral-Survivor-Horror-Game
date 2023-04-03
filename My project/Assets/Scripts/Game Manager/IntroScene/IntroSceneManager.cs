using System.Collections.Generic;
using System.Linq;
using Game_Manager.Mind.COMP499_Project.Game_Scripts;
using UnityEngine;
// using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Game_Manager.IntroScene
{
    public class IntroSceneManager : ZombieManager
    {
        // [Header("Prefabs")]
        // [Tooltip("Prefab for the environment")]
        // [SerializeField] private GameObject environmentPrefab;
        
        [Tooltip("Prefab for the Player")]
        [SerializeField] private GameObject player;

        [Tooltip("Prefab for pickups")]
        [SerializeField] private List<GameObject> pickupsList;
        
        [Tooltip("Transform of the pickup locations")]
        [SerializeField] private List<Transform> pickupLocationsList;
        
        // [Tooltip("Prefab of UI for the player")]
        // [SerializeField] private GameObject playerUI;
        
        [Tooltip("Transform of the end point of the game.")]
        [SerializeField] private Transform endLocation;
        
        private GameObject _playerInstance;

        private readonly List<GameObject> _pickupsInstances = new List<GameObject>();
        // private int _score = 0;

        protected override void Start()
        {
            Setup();
            base.Start();
        }

        private void Setup()
        {
            // Locks the cursor
            Cursor.lockState = CursorLockMode.Locked;
            // Instantiate the environment prefab
            // Instantiate(environmentPrefab);

            // Spawn the player at the player spawn location
            _playerInstance = Instantiate(player, player.transform.position, player.transform.rotation);

            // Spawn the end location detector for moving to next scene
            var transform1 = endLocation.transform;
            Instantiate(endLocation, transform1.position, transform1.rotation);
            
            // Spawn the pickups at the pre-defined locations
            foreach (var pickupInstance in pickupLocationsList.Select(t => Instantiate(pickupsList[Random.Range(0, pickupsList.Count)], t.position, Quaternion.identity)))
            {
                _pickupsInstances.Add(pickupInstance);
            }
        }

        // Function to update the player's score when they collect a pickup
        public void CollectPickup(GameObject pickup)
        {
            // _score += pickup.GetComponent<Pickup>().value;
            // playerUI.GetComponent<PlayerUI>().UpdateScore(_score);
            Destroy(pickup);
        }

        // Function to end the level when the player reaches the end location
        public void EndLevel()
        {
            // Load the next level
            // SceneManager.LoadScene("Level1.0");
        }
    }
}
