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

    public UnityTransport transport;
    
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
            NetworkManager.Singleton.StartHost();
            
            int maxPlayers = 2;

            InLobby.SetActive(false);
            
            StartButtonUI.SetActive(true);
            HostUI.SetActive(true);
            
            string hostIP = GetLocalIpAdress();

            CreateLobbyOptions options = new CreateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {
                        "hostIP", new DataObject(
                            visibility: DataObject.VisibilityOptions.Member, 
                            value: hostIP)
                    }
                }
            };
            
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync("lobbyName", maxPlayers, options);
            
            hostlobby = lobby;
            
            FindObjectOfType<SceneManagers>().ActiveLobby = hostlobby;
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

    public string hostIP;
    public async void joinLobby(string lobbyId)
    {
        try
        {
            Lobby joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);

            hostlobby = joinedLobby;
            
            FindObjectOfType<SceneManagers>().ActiveLobby = hostlobby;
            
            print(hostlobby.Data);
            print(joinedLobby.Data);
            
            if (joinedLobby.Data.TryGetValue("hostIP", out var ipData))
            {
                print("fuck");
                
                hostIP = ipData.Value;
                
                Debug.Log("Joining host at IP: " + hostIP);
            }
            else
            {
                Debug.LogWarning("Host IP not found in lobby data.");
            }
            
            if (transport == null)
            {
                transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            }

            if (transport != null)
            {
                transport.ConnectionData.Address = hostIP;
                Debug.Log("Joining host at IP: " + hostIP);
            }
            else
            {
                Debug.LogError("UnityTransport not found on NetworkManager!");
            }


            NetworkManager.Singleton.StartClient();
            
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
