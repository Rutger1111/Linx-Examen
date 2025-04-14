using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class lobbytest : MonoBehaviour
{

    private Lobby hostlobby;

    private float heartBeatTimer;

    public GameObject Button;

    public GameObject parent;

    public GameObject InLobby;

    public GameObject player;

    public GameObject servers;

    public GameObject ll;
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        string playerId;
    
        if (PlayerPrefs.HasKey("PlayerCustomID"))
        {
            playerId = PlayerPrefs.GetString("PlayerCustomID");
        }
        else
        {
            playerId = "esc" + UnityEngine.Random.Range(0, 100);
            PlayerPrefs.SetString("PlayerCustomID", playerId);
        }

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        Debug.Log("Signed in anonymously.");
        Debug.Log("Custom Player ID: " + playerId);
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
            string lobbyName = "myLobby";
            int maxPlayers = 2;
            
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);

            hostlobby = lobby;

            InLobby.SetActive(false);
            
            ll.SetActive(true);
            
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
            
            servers.SetActive(true);
            
            foreach (Lobby lobby in queryResponse.Results)
            {
                GameObject lobbyButton = Instantiate(Button, parent.transform);
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
            await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            
            print("joined " + lobbyId);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
        /*
        try
        {
            string playerId = Guid.NewGuid().ToString();
            
            var joinedLobbies = await LobbyService.Instance.GetJoinedLobbiesAsync();

            bool isInlobby = false;
            
            foreach (var lobby in joinedLobbies)
            {
                if (lobby == lobbyId)
                {
                    isInlobby = true;
                    
                    break;
                }
            }

            if (isInlobby)
            {
                Debug.Log("player in already in this lobby");
                return;
            }

            var lobbyDetails = await LobbyService.Instance.GetLobbyAsync(lobbyId);

            string lobbyhostId = lobbyDetails.HostId;

            if (lobbyhostId != AuthenticationService.Instance.PlayerId)
            {
                Debug.Log("you are not the host");
                return;
            }
            
            if (joinedLobbies.Count > 0)
            {
                await LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);
            } 
            
            await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            
            servers.SetActive(false);
            ll.SetActive(true);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }*/
    }

    public void playersJoined()
    {
        
    }
}
