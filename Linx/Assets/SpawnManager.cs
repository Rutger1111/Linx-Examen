using System;
using UnityEngine;
using Unity.Netcode;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private Transform[] spawnPoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        if (Unity.Netcode.NetworkManager.Singleton != null)
            Unity.Netcode.NetworkManager.Singleton.OnClientConnectedCallback += onClientConnected;
    }

    private void OnDisable()
    {
        if (Unity.Netcode.NetworkManager.Singleton != null)
            Unity.Netcode.NetworkManager.Singleton.OnClientConnectedCallback -= onClientConnected;
    }

    private void onClientConnected(ulong clientid)
    {
        if (!Unity.Netcode.NetworkManager.Singleton.IsServer) return;
        
        int index = (int)(clientid % (ulong)spawnPoints.Length);

        Transform spawnpoint = spawnPoints[index];

        GameObject playerPrefab = Unity.Netcode.NetworkManager.Singleton.NetworkConfig.PlayerPrefab;

        GameObject playerInstance = Instantiate(playerPrefab, spawnpoint.position, spawnpoint.rotation);
        
        playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientid);
    }
}
