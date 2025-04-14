using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using UnityEngine.UI;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
public class NetworkController : NetworkBehaviour
{
    [SerializeField] private string _gameplayScene = "Game";
    
    [SerializeField] private Button _hostButton;
    
    [SerializeField] private Button _clientButton;

    [SerializeField] private Vector3[] _spawnPoints;
    
    
    private void Awake()
    {
        _hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.StartClient();
            NetworkManager.Singleton.SceneManager.LoadScene(_gameplayScene, LoadSceneMode.Single);
        });
    }
    
    public override void OnNetworkSpawn()
    {
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            transform.position = _spawnPoints[i];
        }
    }
}
