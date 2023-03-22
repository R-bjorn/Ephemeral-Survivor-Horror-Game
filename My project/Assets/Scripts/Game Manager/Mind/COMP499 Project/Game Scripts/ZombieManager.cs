using System.Linq;
using Game_Manager.AI_Scripts;
using UnityEngine;

namespace Game_Manager.Mind.COMP499_Project.Game_Scripts
{
    public class ZombieManager : Manager
    {
        public enum ZombieType
        {
            SlowZombie = 0,
            SwarmZombie,
            RangedZombie,
            FastZombie,
            TankZombie,
            StealthZombie,
            ExplodingZombie
        }
        [Header("Zombie Parameters")] [Tooltip("The hunger to start zombie at.")] [SerializeField]
        private int startingHunger = -100;

        [Tooltip("The maximum hunger before a zombie dies of starvation.")] [SerializeField]
        private int maxHunger = 200;
        
        [Tooltip("The hunger restored from eating a zombie.")]
        [Min(1)]
        [SerializeField]
        private int hungerRestoredFromEating = 100;
        [Tooltip("The minimum number of zombies there must be.")]
        [Min(2)]
        [SerializeField]
        private int minZombies = 10;
        [Tooltip("The maximum number of zombies there can be.")]
        [Min(2)]
        [SerializeField]
        private int maxZombies = 30;
        
        [Tooltip("The slowest speed a zombie can have.")]
        [Min(float.Epsilon)]
        [SerializeField]
        private float minZombiesSpeed = 5f;
        
        [Tooltip("The fastest speed a zombie can have.")]
        [Min(float.Epsilon)]
        [SerializeField]
        private float maxZombiesSpeed = 10f;

        [Tooltip("The shortest lifespan a zombie can have.")] [Min(float.Epsilon)] [SerializeField]
        private float minZombieLifespan = 300f;
        
        [Tooltip("The longest lifespan a zombie can have.")]
        [Min(float.Epsilon)]
        [SerializeField]
        private float maxZombieLifespan = 600f;
        
        [Tooltip("The minimum distance a zombie can detect up to.")]
        [Min(0)]
        [SerializeField]
        private float minZombieDetectionRange = 5;
        
        [Tooltip("How close zombies must be to interact.")]
        [Min(float.Epsilon)]
        [SerializeField]
        private float zombieInteractRadius = 1;
        
        [Tooltip("The chance that a zombie will increase in hunger every tick.")]
        [Min(0)]
        [SerializeField]
        private float hungerChance = 0.05f;
        
        [Header("Zombie Prefab")]
        [Tooltip("Prefab of Zombie")]
        [SerializeField] private GameObject zombiePrefab;
        
        [Tooltip("Transform of locations for enemy object's spawning areas")]
        [SerializeField] private Transform zombieSpawnLocations;
        
        private static ZombieManager ManagerSingleton => Singleton as ZombieManager;
        public static int MaxZombies => ManagerSingleton.maxZombies;
        public static float HungerChange => ManagerSingleton.hungerChance;
        public static int HungerRestoredFromEating => ManagerSingleton.hungerRestoredFromEating;
        public static float MicrobeInteractRadius => ManagerSingleton.zombieInteractRadius;
        public static int StartingHunger => ManagerSingleton.startingHunger;

        public static ZombieCharacter[] Zombies =>
            Singleton.Agents.Where(a => a is ZombieCharacter).Cast<ZombieCharacter>().ToArray();
        
        private Transform[] EnemySpawnLocation => zombieSpawnLocations.GetComponentsInChildren<Transform>();

        public static double PlayerInteractionRadius { get; set; }
        public static int HungerRestoredFromEatingPlayer { get; set; }
        public static double ZombieInteractRadius { get; set; }
        public static float HungerRestoredFromEatingZombie { get; set; }
        public static double HungerChance { get; set; }
        
        protected override void Start()
        {
            ResetAgents();
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
            
            // Loop through all zombies.
            for (int i = 0; i < Agents.Count; i++)
            {
                // There should never be any that are not microbes but check just in case.
                if (!(Agents[i] is ZombieCharacter zombie))
                    continue;

                // Increment the lifespan.
                zombie.Age();

                // If a microbe has not starved, not died of old age, and has not gone out of bounds, update its size to reflect its age.
                if (zombie.Hunger <= maxHunger && zombie.ElapsedLifespan < zombie.LifeSpan)
                {
                    continue;
                }

                // Otherwise, kill the microbe.
                zombie.Die();
                i--;
            }
            
            // Ensure there are enough microbes in the level.
            while (Agents.Count < minZombies)
                SpawnZombie();
            
        }

        protected override float CustomRendering(float x, float y, float w, float h, float p)
        {
            // Regenerate the floor button.
            if (GuiButton(x, y, w, h, "Reset"))
            {
                ResetAgents();
                ClearMessages();
            }
            
            return NextItem(y, h, p);
        }
        private void ResetAgents()
        {
            for (int i = Agents.Count - 1; i >= 0; i--)
            {
                Destroy(Agents[i].gameObject);
            }
            
            // Spawn the enemies at the pre-defined locations
            foreach (var enemyInstance in EnemySpawnLocation)
            {
                if (enemyInstance == zombieSpawnLocations.transform) continue;
                SpawnZombie(enemyInstance);
            }
        }

        private void SpawnZombie()
        {
            int randomIndex = Random.Range(0, EnemySpawnLocation.Length - 1);
            SpawnZombie(EnemySpawnLocation[randomIndex]);
        }
        private void SpawnZombie(Transform enemyInstance)
        {
            SpawnZombie(ZombieType.SlowZombie, enemyInstance.position, Random.Range(minZombiesSpeed, maxZombiesSpeed), Random.Range(minZombieLifespan, maxZombieLifespan), minZombieDetectionRange);
        }

        private static void SpawnZombie(ZombieType zombieType, Vector3 position, float moveSpeed, float lifespan,
            float detectionRange)
        {
            if (ManagerSingleton.Agents.Count >= ManagerSingleton.maxZombies)
            {
                return;
            }
            
            //Setup the zombie character
            GameObject go = Instantiate(ManagerSingleton.zombiePrefab, position,Quaternion.identity);
            ZombieCharacter zombie = go.GetComponent<ZombieCharacter>();
            if (zombie == null)
                return;

            zombie.ZombieType = zombieType;
            zombie.SetHunger(ManagerSingleton.startingHunger);
            zombie.SetLifeSpan(lifespan);
            zombie.SetDetectionRange(detectionRange);
            zombie.SetMoveSpeed(moveSpeed);

            string n = "Zombie";
            Agent[] zombieInScene = ManagerSingleton.Agents.Where(a => a is ZombieCharacter z && z != zombie).ToArray();
            if (zombieInScene.Length == 0)
                zombie.name = $"{n} 1";
            for (int i = 1;; i++)
            {
                if (zombieInScene.Any(m => m.name == $"Zombie {i}"))
                    continue;
                n = $"{n} {i}";
                zombie.name = n;
                break;
            }
            SortAgents();
            GlobalLog($"Spawned zombie {n}");
        }
        
       
    }
}
