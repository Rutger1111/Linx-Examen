using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;
using Random = System.Random;

public class connectToServers : MonoBehaviourPunCallbacks
{
    public static connectToServers instance;
    
    public TMP_InputField roomnameInputField;
    public TMP_Text errorText, roomnameText;
    
    public Transform RoomListContainer;
    public GameObject RoomListPrefab;
    public Transform PlayerListContainer;
    public GameObject PlayerListPrefab;
    public GameObject startGameButton;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        menuManager.instance.OpenMenu("Title");
        Debug.Log("joined lobby");

        PhotonNetwork.NickName = "player";
        
        //here names
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomnameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomnameInputField.text);
        menuManager.instance.OpenMenu("Loading");
    }

    public override void OnJoinedRoom()
    {
        menuManager.instance.OpenMenu("Room");
        roomnameText.text = PhotonNetwork.CurrentRoom.Name;
        
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Transform child in PlayerListContainer)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(PlayerListPrefab, PlayerListContainer).GetComponent<PlayerListItem>().setup(players[i]);
        }
        
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed" + message;
        menuManager.instance.OpenMenu("Error");
    }

    public void startGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
    
    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        menuManager.instance.OpenMenu("Loading");
    }

    public override void OnLeftRoom()
    {
        menuManager.instance.OpenMenu("Title");
    }

    public void joinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        menuManager.instance.OpenMenu("Loading");
        
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in RoomListContainer)
        {
            Destroy(trans.gameObject);
        }
        
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList) 
                continue;    
            
            Instantiate(RoomListPrefab, RoomListContainer).GetComponent<RoomListITem>().setup(roomList[i]);
                
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListPrefab, PlayerListContainer).GetComponent<PlayerListItem>().setup(newPlayer);
    }
}
