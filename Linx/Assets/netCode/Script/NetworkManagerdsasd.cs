using System;
using TMPro;
using UnityEngine;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Unity.Netcode;

public class NetworkManagerdsasd : MonoBehaviour
{
    
    [SerializeField] 
    private string _gameplayScene = "Game";
    
    [SerializeField] 
    private TMP_InputField _ipInputField;
    
    private string _ipAddress = "0.0.0.0";
    private ushort _port = 7777;
    
    /*
    private void Awake()
    {
        Unity.Netcode.NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }

    private void OnDestroy()
    {
        if (Unity.Netcode.NetworkManager.Singleton != null)
        {
            Unity.Netcode.NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
        }
    }

    
    private void OnServerStarted()
    {
        Unity.Netcode.NetworkManager.Singleton.NetworkConfig.PlayerPrefab = null;
    }*/
    
    ///</>summary>
    /// Start host is connectable for a button
    ///</>summary>
    public void StartHost()
    {
        try
        {
            var networkManager = Unity.Netcode.NetworkManager.Singleton;
            networkManager.GetComponent<UnityTransport>().SetConnectionData(_ipAddress, _port);

            
            
            if (networkManager.StartHost())
            {
                networkManager.SceneManager.LoadScene(_gameplayScene, LoadSceneMode.Single);
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }
    ///</>summary>
    /// Start server is connectable for a button
    ///</>summary>
    public void StartServer()
    {
        try
        {
            Unity.Netcode.NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(_ipAddress, _port);

            if (Unity.Netcode.NetworkManager.Singleton.StartServer())
            {
                Unity.Netcode.NetworkManager.Singleton.SceneManager.LoadScene(_gameplayScene, LoadSceneMode.Single);
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }
    ///</>summary>
    /// Start client is connectable for a button
    ///</>summary>
    public void StartClient()
    {
        try
        {
            var networkManager = Unity.Netcode.NetworkManager.Singleton;
            
            string ConnectIp = _ipInputField.text;

            Unity.Netcode.NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ConnectIp, _port);
            
            networkManager.StartClient();
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
