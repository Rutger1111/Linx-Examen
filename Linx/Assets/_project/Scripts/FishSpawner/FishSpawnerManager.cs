using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace FishSpawner
{
    public class FishSpawnerManager : NetworkBehaviour
    {
        [SerializeField] private List<SpawnContainer> spawnInformation = new List<SpawnContainer>();
        
        [SerializeField] private GameObject _playerRefence; // Reference to the player to check proximity


        private void Start()
        {
            if (IsServer)
            {
                foreach (SpawnContainer con in spawnInformation)
                {
                    if (con != null) 
                    {
                        con.containerBehaviour.spawnCoolDown.Value = con.spawnCoolDownReset;
                    }
                    else
                    {
                        Debug.LogError("SpawnContainer is null in spawnInformation list!");
                    }
                }
            }
        }

        void Update()
        {
            if (IsServer)
            {
                
                SpawnHandle();    
            }
        }

        void SpawnHandle()
        {
            foreach (SpawnContainer con in spawnInformation)
            {
                // Remove any null or destroyed fish from the list
                con.enemiesSpawned.RemoveAll(enemy => enemy == null);
                
                // Check if the player is far enough from the spawn point to allow spawning
                if (Vector3.Distance(con.spawnPoint.transform.position, _playerRefence.transform.position) > 10f && con.hasStartedSpawn == true)
                {
                    // Ensure the number of spawned fish does not exceed the max limit
                    if (con.enemiesSpawned.Count + 1 <= con.maxEnemies)
                    {
                        con.containerBehaviour.spawnCoolDown.Value -= Time.deltaTime;
                        
                        // Spawn a fish when cooldown reaches zero
                        if (con.containerBehaviour.spawnCoolDown.Value <= 0)
                        {
                            con.containerBehaviour.spawnCoolDown.Value = con.spawnCoolDownReset; //reset cooldown
                            
                            InitializeSpawning(con);
                        }
                    }
                }
                
                // Start initial spawning if it hasn't been triggered yet
                if (con.hasStartedSpawn == false)
                {
                    StartSpawnHandle(con);
                }
            }
        }
        
        
        void StartSpawnHandle(SpawnContainer con)
        {
            
            // Spawn all fish immediately at the start
            if (con.enemiesSpawned.Count + 1 <= con.maxEnemies)
            {
                InitializeSpawning(con);
            }
            else
            {
                con.hasStartedSpawn = true; // Mark as completed initial spawn
            }
            
        }
        
        
        void InitializeSpawning(SpawnContainer con)
        {
            if (con.enemiesSpawned.Count + 1 <= con.maxEnemies)
            {
                // Generate a random position within the spawn area
                float randomX = Random.Range(-con.spawnArea.x / 2f, con.spawnArea.x / 2f);
                float randomY = Random.Range(-con.spawnArea.y / 2f, con.spawnArea.y / 2f);
                Vector3 randomPosition = con.spawnPoint.transform.position + new Vector3(randomX, randomY);

                // Instantiate the enemy and set hierarchy
                GameObject enemy = Instantiate(con.enemyPrefab, randomPosition, Quaternion.identity);
                enemy.GetComponent<NetworkObject>().Spawn();
                con.enemiesSpawned.Add(enemy);

                NetworkObject enemyNetworkObject = enemy.GetComponent<NetworkObject>();
                
                enemy.transform.parent = con.spawnParent.transform;
                enemyNetworkObject.TrySetParent(con.spawnParent);
            }
        }

        private void OnDrawGizmos()
        {
            // Draw a wireframe cube to visualize the spawn area in the editor
            foreach (SpawnContainer con in spawnInformation)
            {
                Gizmos.DrawWireCube(con.spawnPoint.transform.position, con.spawnArea);
            }
        }
    }

    [Serializable]
    public class SpawnContainer
    {
        public GameObject spawnPoint; // Reference to the spawn location
        public GameObject enemyPrefab; // Prefab of the enemy to spawn
        public Vector2 spawnArea; // Size of the spawnable area

        public SpawnContainerBehaviour containerBehaviour;
        public float spawnCoolDownReset; // Time to reset the cooldown

        public List<GameObject> enemiesSpawned = new List<GameObject>(); // List of currently spawned enemies
        public int maxEnemies; // Maximum number of enemies allowed

        public GameObject spawnParent; // Parent object to keep hierarchy organized

        public bool hasStartedSpawn; // Flag to check if initial spawning is done
        
    }
}
