using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class lobbytest : NetworkBehaviour
{

    [SerializeField] private string _gameplayScene = "Game";
    
    private Lobby hostlobby;

    private float heartBeatTimer;

    public GameObject Buttons;
    public GameObject playersJoinedListPrefab;

    public GameObject parent, parent2;

    public GameObject InLobby;

    public GameObject player;

    public GameObject serverContainer;

    public GameObject HostUI;
    public GameObject StartButtonUI;

    public GameObject playersInServerContainer;

    
    
    public bool HasCreatedLobby = false;
    
    private void Awake()
    {
        
    }
    
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        
        AuthenticationService.Instance.ClearSessionToken();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        
        Debug.Log("Signed in anonymously. Player ID: " + AuthenticationService.Instance.PlayerId);
    }

    private void Update()
    {
        handleLobbyHeartBeat();
    }

    private async void handleLobbyHeartBeat()
    {
        if (hostlobby != null)
        {
            heartBeatTimer -= Time.deltaTime;

            if (heartBeatTimer < 0)
            {
                float heartBeatTimerMax = 15;
                
                heartBeatTimer = heartBeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostlobby.Id);
            }
        }
    }

    public async void createLobby()
    {
        try
        {
            NetworkManager.Singleton.StartHost();
            
            string lobbyName = "myLobby";
            int maxPlayers = 2;
            
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);

            hostlobby = lobby;

            InLobby.SetActive(false);
            
            StartButtonUI.SetActive(true);
            HostUI.SetActive(true);
            
            
            playersJoined();

            Debug.Log("created lobby! " + lobby.Name + " " + lobby.MaxPlayers);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void ListLobbies()
    {
        try
        {
            foreach (Transform child in parent.transform)
            {
                Destroy(child.gameObject);
            }
            
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions()
            {
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)    
                },
                Order = new List<QueryOrder>
                {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };
            
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            
            serverContainer.SetActive(true);
            
            foreach (Lobby lobby in queryResponse.Results)
            {
                GameObject lobbyButton = Instantiate(Buttons, parent.transform);
                lobbyButton.GetComponent<LobbyButtonUI>().Setup(lobby.Name, lobby.Id);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void joinLobby(string lobbyId)
    {
        try
        {
            NetworkManager.Singleton.StartClient();
            
            await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            
            InLobby.SetActive(false);
            
            HostUI.SetActive(true);
            StartButtonUI.SetActive(true);
            
            serverContainer.SetActive(false);

            print("joined " + lobbyId);
            
            playersJoined();
            
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void playersJoined()
    {
        
            try
            {
                QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync();

                Debug.Log("Players in lobby: " + queryResponse.Results.Count);
            
                playersInServerContainer.SetActive(true);
            
                foreach (Lobby lobby in queryResponse.Results)
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
        NetworkManager.Singleton.SceneManager.LoadScene(_gameplayScene, LoadSceneMode.Single);
    }
}
