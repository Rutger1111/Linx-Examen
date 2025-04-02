using System;
using TMPro;
using UnityEngine;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class NetworkManager : MonoBehaviour
{
    
    [SerializeField] 
    private string _gameplayScene = "Game";
    
    [SerializeField] 
    private TMP_InputField _ipInputField;
    
    private string _ipAddress = "0.0.0.0";
    private ushort _port = 7777;

    ///</>summary>
    /// Start host is connectable for a button
    ///</>summary>
    public void StartHost()
    {
        try
        {
            Unity.Netcode.NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(_ipAddress, _port);

            if (Unity.Netcode.NetworkManager.Singleton.StartHost())
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
            string ConnectIp = _ipInputField.text;
            
            Unity.Netcode.NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ConnectIp, _port);
            Unity.Netcode.NetworkManager.Singleton.StartClient();
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
