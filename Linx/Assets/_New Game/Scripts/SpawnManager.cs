using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] private GameObject redPlayer;
    [SerializeField] private GameObject bluePlayer;

    [SerializeField] private Vector3 spawnPointRed;
    [SerializeField] private Vector3 spawnPointBlue;
    
    
    [SerializeField] private List<GameObject> _playerSpawned = new List<GameObject>();

    private bool _hasDestroyedWithSpawner;
    public Lobby _activeLobby;
    private Scene _currentScene;
    private GameObject _prefabInstance;
    private NetworkObject _spawnedNetworkObject;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        _currentScene = SceneManager.GetActiveScene();
        
        if (IsServer && _currentScene.name == "Multiplayer" && _playerSpawned.Count < _activeLobby.Players.Count)
        {
            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                bool alreadySpawned = _playerSpawned.Exists(p =>
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
        int spawnIndex = _playerSpawned.Count;

        if (spawnIndex == 0)
        {
            RedPlayerSpawn(clientId);
        }
        else if (spawnIndex == 1)
        {
            BluePlayerSpawn(clientId);
        }
        else
        {
            Debug.LogWarning($"No spawn logic for player index {spawnIndex}. Add more prefabs if needed.");
        }
       
    }

    public void RedPlayerSpawn(ulong clientId)
    {
        GameObject playerInstance = Instantiate(redPlayer);
        playerInstance.transform.position = spawnPointRed;
        var netObj = playerInstance.GetComponent<NetworkObject>();
        if (netObj != null)
        {
            netObj.SpawnWithOwnership(clientId);
            _playerSpawned.Add(playerInstance);
            
            var info = playerInstance.GetComponent<PlayerInfo>();
            if (info != null)
            {
                info.SetClientId(clientId);
            }
        }
        else
        {
            Debug.LogError("Player prefab does not have a NetworkObject attached.");
        }
    }

    public void BluePlayerSpawn(ulong clientId)
    {
        GameObject playerInstance = Instantiate(bluePlayer);
        playerInstance.transform.position = spawnPointBlue;
        
        var netObj = playerInstance.GetComponent<NetworkObject>();
        if (netObj != null)
        {
            netObj.SpawnWithOwnership(clientId);
            _playerSpawned.Add(playerInstance);
            
            var info = playerInstance.GetComponent<PlayerInfo>();
            if (info != null)
            {
                info.SetClientId(clientId);
            }
        }
        else
        {
            Debug.LogError("Player prefab does not have a NetworkObject attached.");
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