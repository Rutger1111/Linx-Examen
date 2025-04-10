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

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
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

    public async void joinLobby(string lobbyId, string playerId)
    {
        try
        {
            var joinedLobbies = await LobbyService.Instance.GetJoinedLobbiesAsync();

            foreach (var lobby in joinedLobbies)
            {
                if (lobby == lobbyId)
                {
                    Debug.Log("player is already in this lobby");
                    return;
                }  
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
        }
    }

    public void playersJoined()
    {
        
    }
}
