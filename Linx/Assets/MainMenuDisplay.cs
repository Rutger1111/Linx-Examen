using System;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;
using Unity.Networking.Transport;
using UnityEngine.UI;
using System.Net;
public class MainMenuDisplay : MonoBehaviour
{
    [SerializeField] private string GamePlayScene = "Game";

    public string ipAddress = "0.0.0.0";
    public ushort port = 7777;

    public TMP_InputField ipInputField;

    public DebugTextCollector _debug;

    private void Start()
    {
        _debug = DebugTextCollector.GetTextCollector();
    }

    public void startHost()
    {
        try
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipAddress, port);

            if (NetworkManager.Singleton.StartHost())
            {
                _debug.AddDebugText("maak host");
                print("ipAddress" + ipAddress);
                print(port);
                
                string strHostName = "Server";
                strHostName = System.Net.Dns.GetHostName();
                var ipEntry =Dns.GetHostEntry(strHostName);
                var addr = ipEntry.AddressList;
                _debug.AddDebugText("used" + IPAddress.Parse(addr[addr.Length - 1].ToString()) + "hoi");
                
                
                NetworkManager.Singleton.SceneManager.LoadScene(GamePlayScene, LoadSceneMode.Single);
            }
            //NetworkManager.Singleton.StartHost();
        }
        catch (Exception e)
        {
            _debug.AddDebugText(""+e);
            throw;
        }
       
       
    }

    public void StartServer()
    {
        try
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipAddress, port);

            if (NetworkManager.Singleton.StartServer())
            {
                _debug.AddDebugText("Dedicated Server Started");
                Debug.Log("Server started on " + ipAddress + ":" + port);

                NetworkManager.Singleton.SceneManager.LoadScene(GamePlayScene, LoadSceneMode.Single);
            }
        }
        catch (Exception e)
        {
            _debug.AddDebugText("Error: " + e);
        }
    }
    
    public void StartClient()
    {
        try
        {
            string ConnectIp = ipInputField.text;
        
            _debug.AddDebugText("ga aan");
        
        
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ConnectIp, port);
            NetworkManager.Singleton.StartClient();
            
            _debug.AddDebugText("" + ConnectIp);
            _debug.AddDebugText(""+ NetworkManager.Singleton.StartClient());
            
            string strHostName = "Server";
            strHostName = System.Net.Dns.GetHostName();
            var ipEntry =Dns.GetHostEntry(strHostName);
            var addr = ipEntry.AddressList;
            _debug.AddDebugText("used" + IPAddress.Parse(addr[addr.Length - 1].ToString()) + "hoi");
        }
        catch (Exception e)
        {
            _debug.AddDebugText(""+e);
            
        }
        
    }
}
