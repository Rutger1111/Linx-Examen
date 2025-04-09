using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private string gameplaySceneName = "Game";

    private HashSet<ulong> clientsToSpawn = new HashSet<ulong>();

    private void Awake()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.SceneManager.OnLoadComplete += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null) return;

        NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnSceneLoaded;
    }

    private void OnServerStarted()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            // Host is also a client
            clientsToSpawn.Add(NetworkManager.Singleton.LocalClientId);
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            clientsToSpawn.Add(clientId);
        }
    }

    private void OnSceneLoaded(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        if (sceneName != gameplaySceneName) return;

        if (clientsToSpawn.Contains(clientId))
        {
            Vector3 spawnPosition = GetSpawnPositionForClient(clientId);
            GameObject playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

            NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();
            networkObject.SpawnWithOwnership(clientId);

            clientsToSpawn.Remove(clientId);
        }
    }

    private Vector3 GetSpawnPositionForClient(ulong clientId)
    {
        // Space out players on X axis based on their client ID
        return new Vector3(clientId * 2f, 0f, 0f);
    }
}
