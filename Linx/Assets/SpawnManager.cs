using System;
using UnityEngine;
using Unity.Netcode;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject playerPrefab;

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
        {
            Unity.Netcode.NetworkManager.Singleton.OnClientConnectedCallback += onClientConnected;
        }
           
    }

    private void OnDisable()
    {
        if (Unity.Netcode.NetworkManager.Singleton != null)
            Unity.Netcode.NetworkManager.Singleton.OnClientConnectedCallback -= onClientConnected;
    }

    private void onClientConnected(ulong clientid)
    {
        
        if (!Unity.Netcode.NetworkManager.Singleton.IsServer) return;

        
        if (clientid == 0)
        {
            SpawnPlayer(clientid);
        }
        else
        {
            SpawnPlayer(clientid);
        }
    }

    public void SpawnPlayer(ulong clientid)
    {
        int index = (int)(clientid % (ulong)spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];
        
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab not assigned in SpawnManager.");
            return;
        }
        
        GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        
        NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.SpawnAsPlayerObject(clientid);
        }
        
    }
}
