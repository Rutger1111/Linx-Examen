using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace FishSpawner
{
    public class FishSpawnerManager : MonoBehaviour
    {
        [SerializeField] private List<SpawnContainer> spawnInformation = new List<SpawnContainer>();

        [SerializeField] private GameObject _playerRefence;

        void Update()
        {
            SpawnHandle();
        }

        void SpawnHandle()
        {
            foreach (SpawnContainer con in spawnInformation)
            {
                con.enemiesSpawned.RemoveAll(enemy => enemy == null);
                if (Vector3.Distance(con.spawnPoint.transform.position, _playerRefence.transform.position) > 10f &&
                    con.hasStartedSpawn == true)
                {
                    if (con.enemiesSpawned.Count + 1 <= con.maxEnemies)
                    {
                        con.spawnCoolDown -= Time.deltaTime;
                        if (con.spawnCoolDown <= 0)
                        {
                            con.spawnCoolDown = con.spawnCoolDownReset;

                            InitializeSpawning();
                        }
                    }
                }

                if (con.hasStartedSpawn == false)
                {
                    StartSpawnHandle();
                }
            }
        }

        void StartSpawnHandle()
        {
            foreach (SpawnContainer con in spawnInformation)
            {
                con.enemiesSpawned.RemoveAll(enemy => enemy == null);

                if (con.enemiesSpawned.Count + 1 <= con.maxEnemies)
                {
                    InitializeSpawning();
                }
                else
                {
                    con.hasStartedSpawn = true;
                }
            }
        }

        void InitializeSpawning()
        {
            foreach (SpawnContainer con in spawnInformation)
            {
                if (con.enemiesSpawned.Count + 1 <= con.maxEnemies)
                {
                    float randomX = Random.Range(-con.spawnArea.x / 2f, con.spawnArea.x / 2f);
                    float randomY = Random.Range(-con.spawnArea.y / 2f, con.spawnArea.y / 2f);

                    Vector3 randomPosition = con.spawnPoint.transform.position + new Vector3(randomX, randomY);

                    GameObject enemy = Instantiate(con.enemyPrefab, randomPosition, Quaternion.identity);

                    con.enemiesSpawned.Add(enemy);
                    enemy.transform.parent = con.spawnParent.transform;
                }
            }
        }

        private void OnDrawGizmos()
        {
            foreach (SpawnContainer con in spawnInformation)
            {
                Gizmos.DrawWireCube(con.spawnPoint.transform.position, con.spawnArea);
            }
        }
    }

    [Serializable]
    public class SpawnContainer
    {
        public GameObject spawnPoint;
        public GameObject enemyPrefab;
        public Vector2 spawnArea;

        public float spawnCoolDown;
        public float spawnCoolDownReset;

        public List<GameObject> enemiesSpawned;
        public int maxEnemies;

        public GameObject spawnParent;

        public bool hasStartedSpawn;
    }
}
