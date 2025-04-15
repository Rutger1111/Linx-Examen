using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : NetworkBehaviour
{
    public Scene currentScene;

    public Lobby ActiveLobby;
    
    public bool destroyWIthSpawner;
    public GameObject PrefabToSpawn;
    public bool DestroyWithSpawner;
    private GameObject m_PrefabInstance;
    private NetworkObject m_SpawnedNetworkObject;

    public List<GameObject> PlayersSpawned = new List<GameObject>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        currentScene = SceneManager.GetActiveScene();

        // Server-side: make sure we only spawn once per player
        if (IsServer && currentScene.name == "Multiplayer" && PlayersSpawned.Count < ActiveLobby.Players.Count)
        {
            foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            {
                // Check if this client already has a spawned player
                bool alreadySpawned = PlayersSpawned.Exists(p =>
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
        if (PrefabToSpawn == null)
        {
            Debug.LogError("Player Prefab is not assigned.");
            return;
        }

        GameObject playerInstance = Instantiate(PrefabToSpawn);
        playerInstance.transform.position = new Vector3(0, 0, 0); // You can customize this later

        var netObj = playerInstance.GetComponent<NetworkObject>();
        if (netObj != null)
        {
            netObj.SpawnWithOwnership(clientId);
            PlayersSpawned.Add(playerInstance);

            // Assign the owner ID to a custom script
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

        if (IsServer && DestroyWithSpawner && m_SpawnedNetworkObject != null && m_SpawnedNetworkObject.IsSpawned)
        {
            m_SpawnedNetworkObject.Despawn();
        }
    }
}