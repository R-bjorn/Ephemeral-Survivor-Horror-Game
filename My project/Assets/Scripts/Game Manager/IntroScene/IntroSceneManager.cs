using System.Collections.Generic;
using System.Linq;
using Game_Manager.AI_Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game_Manager.IntroScene
{
    public class IntroSceneManager : Manager
    {
        private static IntroSceneManager _introSceneSingleton = Singleton as IntroSceneManager;
        
        // Serialized private variables for assigning in Unity
        [SerializeField] private GameObject environmentPrefab;
        [SerializeField] private GameObject player;
        [SerializeField] private Transform playerSpawnLocation;
        [SerializeField] private List<GameObject> pickupsList;
        [SerializeField] private List<Transform> pickupLocationsList;
        [SerializeField] private GameObject enemyObject;
        // [SerializeField] private State enemyMindControl;
        [SerializeField] private Transform enemySpawnLocations;
        [SerializeField] private GameObject playerUI;
        [SerializeField] private Transform endLocation;
        
        private GameObject _environmentInstance;
        private GameObject _playerInstance;
        private Transform _endLocationInstance;
        private Transform[] EnemySpawnLocation => enemySpawnLocations.GetComponentsInChildren<Transform>();
        private readonly List<GameObject> _pickupsInstances = new List<GameObject>();
        private readonly List<GameObject> _enemiesInstances = new List<GameObject>();
        // private int _score = 0;

        protected override void Start()
        {
            Setup();
        }

        private void Setup()
        {
            // Locks the cursor
            Cursor.lockState = CursorLockMode.Locked;
            // Instantiate the environment prefab
            _environmentInstance = Instantiate(environmentPrefab);

            // Spawn the player at the player spawn location
            _playerInstance = Instantiate(player, playerSpawnLocation.position, playerSpawnLocation.rotation);

            // Spawn the end location detector for moving to next scene
            _endLocationInstance =
                // ReSharper disable once Unity.InefficientPropertyAccess
                Instantiate(endLocation, endLocation.transform.position, endLocation.transform.rotation);
            
            // Spawn the pickups at the pre-defined locations
            foreach (var pickupInstance in pickupLocationsList.Select(t => Instantiate(pickupsList[Random.Range(0, pickupsList.Count)], t.position, Quaternion.identity)))
            {
                _pickupsInstances.Add(pickupInstance);
            }

            
            // // Spawn the enemies at the pre-defined locations
            // foreach (var enemyInstance in EnemySpawnLocation.Select(t => Instantiate(enemyObject, t.position, t.rotation)))
            // {
            //     _enemiesInstances.Add(enemyInstance);
            // }
            
            // Spawn the enemies at the pre-defined locations
            foreach (var enemyInstance in EnemySpawnLocation)
            {
                if (enemyInstance == enemySpawnLocations.transform) continue;
                GameObject enemy = Instantiate(enemyObject, enemyInstance.position, enemyInstance.rotation);
                _enemiesInstances.Add(enemy);
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
            // Stop controlling the enemy mind
            // enemyMindControl.GetComponent<EnemyMindControl>().StopControl();

            // Destroy all remaining pickups and enemies
            foreach (GameObject pickupInstance in _pickupsInstances)
            {
                Destroy(pickupInstance);
            }
            foreach (GameObject enemyInstance in _enemiesInstances)
            {
                Destroy(enemyInstance);
            }

            // Load the next level
            SceneManager.LoadScene("Level1.0");
        }
    }
}
