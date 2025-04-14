using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : NetworkBehaviour
{
    public Scene currentScene;
    
    public GameObject PrefabToSpawn;
    public bool DestroyWithSpawner;
    private GameObject m_PrefabInstance;
    private NetworkObject m_SpawnedNetworkObject;

    public List<GameObject> PlayersSpawned = new List<GameObject>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /*
    void OnEnable()
    {
        
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;    
        
    }

    void OnDisable()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;    
        }    
    }

    private void OnClientConnected(ulong clientId)
    {
        currentScene = SceneManager.GetActiveScene();

        if (!NetworkManager.Singleton.IsServer) return; 

       
        if (currentScene.name == "Multiplayer")
        {
            SpawnPlayer(clientId);
        }
        else
        {
            
            Debug.LogWarning("Player joined before scene was ready.");
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        GameObject playerInstance = Instantiate(player);
        NetworkObject netObj = playerInstance.GetComponent<NetworkObject>();
        netObj.SpawnAsPlayerObject(clientId); 
    }*/

    private void Update()
    {
        currentScene = SceneManager.GetActiveScene();


        if (PlayersSpawned.Count < 2 && currentScene.name == "Multiplayer")
        {
            print("fuck");
            OnNetworkSpawn();    
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        
        if (!IsServer) return;

        
        if (currentScene.name == "Multiplayer")
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        if (PrefabToSpawn == null)
        {
            Debug.LogError("Player Prefab is not assigned.");
            return;
        }
        
        
            GameObject playerInstance = Instantiate(PrefabToSpawn);
            PlayersSpawned.Add(playerInstance);
            playerInstance.transform.position = new Vector3(0,0,0);
            playerInstance.transform.rotation = transform.rotation;

        
            NetworkObject netObj = playerInstance.GetComponent<NetworkObject>();
            if (netObj != null)
            {
                netObj.SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
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
