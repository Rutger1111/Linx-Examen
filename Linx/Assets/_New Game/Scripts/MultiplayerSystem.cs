using System.Collections.Generic;
using _New_Game.Scripts;
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
using UnityEngine.Serialization;

public class MultiplayerSystem : NetworkBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string _gameplayScene = "Game";
    
    
    [Header("Prefabs")]
    [SerializeField] private GameObject _joinButtonsPrefab;
    [SerializeField] private GameObject _joinButtonParent;
    
    
    [SerializeField] private GameObject _playersJoinedPrefab;
    [SerializeField] private GameObject _playersJoinedParent;
    
    
    [Header("UI")]
    [SerializeField] private GameObject _multiplayerUI;
    [SerializeField] private GameObject _lobbyList;
    [SerializeField] private GameObject _GameUI;
    [SerializeField] private GameObject _startGameButtonUI;
    [SerializeField] private GameObject _logo;
    
    
    private string _joinCode;
    private Lobby _hostLobby;
    private float _heartBeatTimer;

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.ClearSessionToken();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        HandleLobbyHeartBeat();
    }

    private async void HandleLobbyHeartBeat()
    {
        if (_hostLobby != null && _hostLobby.HostId == AuthenticationService.Instance.PlayerId)
        {
            _heartBeatTimer -= Time.deltaTime;

            if (_heartBeatTimer < 0)
            {
                float heartBeatTimerMax = 15;
                _heartBeatTimer = heartBeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(_hostLobby.Id);
            }
        }
    }

    public async void CreateLobby()
    {
        try
        {
            int maxPlayers = 2;

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            _joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            

            RelayServerData relayServerData = AllocationUtils.ToRelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            
            
            CreateLobbyOptions options = new CreateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {
                        "joinCode", new DataObject(
                            visibility: DataObject.VisibilityOptions.Member,
                            value: _joinCode)
                    }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync("lobbyName", maxPlayers, options);
            _hostLobby = lobby;

            FindObjectOfType<SpawnManager>().ActiveLobby = _hostLobby;
            

            NetworkManager.Singleton.StartHost();
            
            PlayersJoined();
            
            _multiplayerUI.SetActive(false);
            _startGameButtonUI.SetActive(true);
            _GameUI.SetActive(true);
            _logo.SetActive(false);
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
            foreach (Transform child in _joinButtonParent.transform)
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

            _multiplayerUI.SetActive(false);
            _lobbyList.SetActive(true);
            _logo.SetActive(false);

            foreach (Lobby lobby in queryResponse.Results)
            {
                GameObject lobbyButton = Instantiate(_joinButtonsPrefab, _joinButtonParent.transform);
                lobbyButton.GetComponent<LobbyButtonUI>().Setup(lobby.Name, lobby.Id);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("Error listing lobbies: " + e);
        }
    }

    public async void JoinLobby(string lobbyId)
    {
        try
        {
            Lobby joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            _hostLobby = joinedLobby;

            if (joinedLobby.Data.TryGetValue("joinCode", out var joinCodeData))
            {
                _joinCode = joinCodeData.Value;

                if (string.IsNullOrWhiteSpace(_joinCode))
                {
                    Debug.LogError("Invalid join code received!");
                    return;
                }

                JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(_joinCode);
                RelayServerData relayServerData = AllocationUtils.ToRelayServerData(allocation, "dtls");

                UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetRelayServerData(relayServerData);

                NetworkManager.Singleton.StartClient();

                FindObjectOfType<SpawnManager>().ActiveLobby = _hostLobby;
            }
            else
            {
                Debug.LogWarning("Join code not found in lobby data.");
            }

            
            _multiplayerUI.SetActive(false);
            _lobbyList.SetActive(false);
            _GameUI.SetActive(true);

            PlayersJoined();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("Error joining lobby: " + e);
        }
    }

    public void PlayersJoined()
    {
        foreach (Transform child in _playersJoinedParent.transform)
        {
            print(child);
            Destroy(child.gameObject);
        }
        
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            GameObject entry = Instantiate(_playersJoinedPrefab, _playersJoinedParent.transform);

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
        PlayersJoined();
    }

    private void HandleClientDisconnected(ulong clientId)
    {
        PlayersJoined();
    }

    public void StartGame()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(_gameplayScene, LoadSceneMode.Single);
        }
    }
}
