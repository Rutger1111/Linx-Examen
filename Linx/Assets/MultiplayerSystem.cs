using System;
using System.Collections.Generic;
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
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MultiplayerSystem : NetworkBehaviour
{

    [SerializeField] private string _gameplayScene = "Game";
    
    [SerializeField] private GameObject Buttons;
    [SerializeField] private GameObject playersJoinedListPrefab;
    [SerializeField] private GameObject parent, parent2;
    [SerializeField] private GameObject InLobby;
    [SerializeField] private GameObject serverContainer;
    [SerializeField] private GameObject HostUI;
    [SerializeField] private GameObject StartButtonUI;
    [SerializeField] private GameObject playersInServerContainer;
    
    private string joinCode;
    private Lobby _hostLobby;
    private float heartBeatTimer;

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.ClearSessionToken();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        handleLobbyHeartBeat();
    }

    private async void handleLobbyHeartBeat()
    {
        if (_hostLobby != null && _hostLobby.HostId == AuthenticationService.Instance.PlayerId)
        {
            heartBeatTimer -= Time.deltaTime;

            if (heartBeatTimer < 0)
            {
                float heartBeatTimerMax = 15;
                heartBeatTimer = heartBeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(_hostLobby.Id);
            }
        }
    }

    public async void createLobby()
    {
        try
        {
            int maxPlayers = 2;

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            

            RelayServerData relayServerData = AllocationUtils.ToRelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            
            CreateLobbyOptions options = new CreateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {
                        "joinCode", new DataObject(
                            visibility: DataObject.VisibilityOptions.Member,
                            value: joinCode)
                    }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync("lobbyName", maxPlayers, options);
            _hostLobby = lobby;

            FindObjectOfType<SceneManagers>().ActiveLobby = _hostLobby;
            

            NetworkManager.Singleton.StartHost();
            
            playersJoined();
            
            
            
            InLobby.SetActive(false);
            StartButtonUI.SetActive(true);
            HostUI.SetActive(true);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("Error creating lobby: " + e);
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

            serverContainer.SetActive(true);

            foreach (Lobby lobby in queryResponse.Results)
            {
                GameObject lobbyButton = Instantiate(Buttons, parent.transform);
                lobbyButton.GetComponent<LobbyButtonUI>().Setup(lobby.Name, lobby.Id);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("Error listing lobbies: " + e);
        }
    }

    public async void joinLobby(string lobbyId)
    {
        try
        {
            Lobby joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            _hostLobby = joinedLobby;

            if (joinedLobby.Data.TryGetValue("joinCode", out var joinCodeData))
            {
                joinCode = joinCodeData.Value;

                if (string.IsNullOrWhiteSpace(joinCode))
                {
                    Debug.LogError("Invalid join code received!");
                    return;
                }

                JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                RelayServerData relayServerData = AllocationUtils.ToRelayServerData(allocation, "dtls");

                UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetRelayServerData(relayServerData);

                NetworkManager.Singleton.StartClient(); // ✅ transport already set

                FindObjectOfType<SceneManagers>().ActiveLobby = _hostLobby;
            }
            else
            {
                Debug.LogWarning("Join code not found in lobby data.");
            }

            
            InLobby.SetActive(false);
            HostUI.SetActive(true);
            StartButtonUI.SetActive(true);
            serverContainer.SetActive(false);
            
            playersJoined();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("Error joining lobby: " + e);
        }
    }

    public void playersJoined()
    {
        foreach (Transform child in parent2.transform)
        {
            Destroy(child.gameObject);
        }

        playersInServerContainer.SetActive(true);

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            GameObject entry = Instantiate(playersJoinedListPrefab, parent2.transform);

            var text = entry.GetComponentInChildren<Text>();
            if (text != null)
            {
                text.text = "Player ID: " + client.ClientId;
            }
        }
    }

    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        if (NetworkManager.Singleton == null) return;
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
    }

    private void HandleClientConnected(ulong clientId)
    {
        playersJoined();
    }

    private void HandleClientDisconnected(ulong clientId)
    {
        playersJoined();
    }

    public void startGame()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(_gameplayScene, LoadSceneMode.Single);
        }
        else
        {
            Debug.Log("Client is ready.");
        }
    }
}
