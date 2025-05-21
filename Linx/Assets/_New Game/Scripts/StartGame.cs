using System;
using System.Collections.Generic;
using Unity.Multiplayer.Widgets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private string _gameplayScene = "Multiplayer";
    
    public GameObject playersJoinedListPrefab;

    public GameObject parent2;
    

    private void Update()
    {
        playersJoined();
    }

    public void playersJoined()
    {
            try
            {
                foreach (Transform child in parent2.transform)
                {
                    Destroy(child.gameObject);
                }

               
                int connectedPlayersCount = NetworkManager.Singleton.ConnectedClientsList.Count;
                Debug.Log("Number of players in the lobby: " + connectedPlayersCount);

               
                foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
                {
                    Instantiate(playersJoinedListPrefab, parent2.transform);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        
    }

    public void startGame()
    {
        Debug.Log("StartGame called. IsServer: " + NetworkManager.Singleton.IsServer);

        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log("Loading scene: " + _gameplayScene);
            NetworkManager.Singleton.SceneManager.LoadScene(_gameplayScene, LoadSceneMode.Single);
        }
        else
        {
            Debug.Log("Client tried to start game — ignored.");
        }
    }
    

    
    
}
