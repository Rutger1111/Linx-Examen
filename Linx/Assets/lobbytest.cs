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
    
    public string hostIP;

    public GameObject serverContainer;

    public GameObject HostUI;
    public GameObject StartButtonUI;

    public GameObject playersInServerContainer;

    public bool HasCreatedLobby = false;

    public UnityTransport transport;

    public string joinCode;

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

    private async void Update()
    {
        handleLobbyHeartBeat();

        if (IsLocalPlayer)
        {
            int connectedPlayersCount = NetworkManager.Singleton.ConnectedClientsList.Count;
            Debug.Log("Number of players in the lobby: " + connectedPlayersCount);    
        }
    }

    private async void handleLobbyHeartBeat()
    {
        if (hostlobby != null && hostlobby.HostId == AuthenticationService.Instance.PlayerId)
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
            int maxPlayers = 2;
            
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("Join code generated: " + joinCode); // Debugging the join code
            
            RelayServerData relayServerData = AllocationUtils.ToRelayServerData(allocation, "dtls");
            
            NetworkManager.Singleton.StartHost();
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            
            InLobby.SetActive(false);
            StartButtonUI.SetActive(true);
            HostUI.SetActive(true);

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
            hostlobby = lobby;
            
            FindObjectOfType<SceneManagers>().ActiveLobby = hostlobby;
            playersJoined();

            Debug.Log("Created lobby: " + lobby.Name + " with MaxPlayers: " + lobby.MaxPlayers);
            
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
            Debug.LogError("Error listing lobbies: " + e);
        }
    }

    public async void joinLobby(string lobbyId)
    {
        try
        {
            Debug.Log("Attempting to join lobby with ID: " + lobbyId);
            
            Lobby joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            hostlobby = joinedLobby;
            
            if (joinedLobby.Data.TryGetValue("joinCode", out var joinCodeData))
            {
                joinCode = joinCodeData.Value;
                Debug.Log("Join code received: " + joinCode);

                if (string.IsNullOrWhiteSpace(joinCode))
                {
                    Debug.LogError("Invalid join code received!");
                    return;
                }

                Debug.Log("Attempting to join Relay with join code...");
                JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                Debug.Log("Relay allocation successful");

                RelayServerData relayServerData = AllocationUtils.ToRelayServerData(allocation, "dtls");
                
                UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetRelayServerData(relayServerData);
                
                bool result = NetworkManager.Singleton.StartClient();

                if (!result)
                {
                    Debug.LogError("Failed to start client!");
                    return;
                }
            
                FindObjectOfType<SceneManagers>().ActiveLobby = hostlobby;
            }
            else
            {
                Debug.LogWarning("Host IP not found in lobby data.");
            }

            InLobby.SetActive(false);
            HostUI.SetActive(true);
            StartButtonUI.SetActive(true);
            serverContainer.SetActive(false);

            Debug.Log("Joined lobby " + lobbyId);
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

        // Loop through all connected clients
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            GameObject entry = Instantiate(playersJoinedListPrefab, parent2.transform);

            // Optional: Show client ID or something else
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
            Debug.Log("Host starting the game...");
            NetworkManager.Singleton.SceneManager.LoadScene(_gameplayScene, LoadSceneMode.Single);
        }
        else
        {
            Debug.Log("Client is ready.");
        }
    }

    private string GetLocalIpAdress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
}
