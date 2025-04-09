using System;
using TMPro;
using UnityEngine;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Unity.Netcode;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class NetworkManagerdsasd : NetworkBehaviour
{
    
    [SerializeField] 
    private string _gameplayScene = "Game";
    
    
    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;

    public Vector3[] spawnpoints;
    
    
    private void Awake()
    {
        HostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene(_gameplayScene, LoadSceneMode.Single);
        });
        ClientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }

    public override void OnNetworkSpawn()
    {
        for (int i = 0; i < spawnpoints.Length; i++)
        {
            transform.position = spawnpoints[i];
        }
    }
}
