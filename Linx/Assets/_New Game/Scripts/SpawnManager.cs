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

    [SerializeField] private GameObject spawnPointRed;
    [SerializeField] private GameObject spawnPointBlue;
    
    
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

    private void Start()
    {
        _currentScene = SceneManager.GetActiveScene();
        
        if (IsServer && _currentScene.name == "Multiplayer" && _playerSpawned.Count < _activeLobby.Players.Count)
        {
            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                print(NetworkManager.Singleton.ConnectedClientsList.Count);
                bool alreadySpawned = _playerSpawned.Exists(p =>
                {
                    var info = p.GetComponent<PlayerInfo>();
                    return info != null && info.OwnerClientId == client.ClientId;
                });

                if (!alreadySpawned)
                {
                    SpawnPlayer();
                }
            }
        }
    }

    private void SpawnPlayer()
    {
        if (redPlayer == null || bluePlayer == null)
        {
            Debug.LogError("Player Prefab is not assigned.");
            return;
        }

        RedPlayerSpawn(0);
        BluePlayerSpawn(1);
       
    }

    public void RedPlayerSpawn(ulong clientId)
    {
        GameObject playerInstance = Instantiate(redPlayer, spawnPointRed.transform);
        
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
        GameObject playerInstance = Instantiate(bluePlayer, spawnPointBlue.transform);

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