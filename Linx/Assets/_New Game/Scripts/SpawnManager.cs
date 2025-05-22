using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _New_Game.Scripts
{
    public class SpawnManager : NetworkBehaviour
    {
        [SerializeField] private GameObject redPlayer;
        [SerializeField] private GameObject bluePlayer;

        [SerializeField] private Vector3 spawnPointRed;
        [SerializeField] private Vector3 spawnPointBlue;
    
    
        [FormerlySerializedAs("_playerSpawned")] [SerializeField] private List<GameObject> playerSpawned = new List<GameObject>();

        private bool _hasDestroyedWithSpawner;
        public Lobby ActiveLobby;
        private Scene _currentScene;
        private GameObject _prefabInstance;
        private NetworkObject _spawnedNetworkObject;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (!IsServer) { return; }
        
            _currentScene = SceneManager.GetActiveScene();
        
            if (IsServer && _currentScene.name == "Multiplayer" && playerSpawned.Count < ActiveLobby.Players.Count)
            {
                foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
                {
                
                    bool alreadySpawned = playerSpawned.Exists(p =>
                    {
                        var info = p.GetComponent<PlayerInfo>(); 
                        return info != null && info.OwnerClientId == client.ClientId; 
                    });

                    if (!alreadySpawned) 
                    { 
                        SpawnPlayer(client.ClientId);
                    }
                    
                }
            }
        }

        private void SpawnPlayer(ulong clientId)
        {
            int spawnIndex = playerSpawned.Count;

            switch (spawnIndex)
            {
                case 0:
                    RedPlayerSpawn(clientId);
                    break;
                case 1:
                    BluePlayerSpawn(clientId);
                    break;
            }
        }

        private void RedPlayerSpawn(ulong clientId)
        {
            GameObject playerInstance = Instantiate(redPlayer);
            playerInstance.transform.position = spawnPointRed;
        
            var netObj = playerInstance.GetComponent<NetworkObject>();
            if (netObj != null)
            {
                netObj.SpawnWithOwnership(clientId);
            
                playerSpawned.Add(playerInstance);
            
                var info = playerInstance.GetComponent<PlayerInfo>();
                if (info != null)
                {
                    info.SetClientId(clientId);
                }
            }
        }

        private void BluePlayerSpawn(ulong clientId)
        {
            GameObject playerInstance = Instantiate(bluePlayer);
            playerInstance.transform.position = spawnPointBlue;
        
            var netObj = playerInstance.GetComponent<NetworkObject>();
            if (netObj != null)
            {
                netObj.SpawnWithOwnership(clientId);
            
                playerSpawned.Add(playerInstance);
            
                var info = playerInstance.GetComponent<PlayerInfo>();
                if (info != null)
                {
                    info.SetClientId(clientId);
                }
            }
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            if (IsServer && _hasDestroyedWithSpawner && _spawnedNetworkObject != null && _spawnedNetworkObject.IsSpawned)
            {
                _spawnedNetworkObject.Despawn();
            }
        }
    }
}