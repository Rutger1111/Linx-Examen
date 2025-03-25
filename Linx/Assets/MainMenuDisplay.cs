using TMPro;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;
using Unity.Networking.Transport;
using UnityEngine.UI;

public class MainMenuDisplay : MonoBehaviour
{
    [SerializeField] private string GamePlayScene = "Game";

    public string ipAddress = "0.0.0.0";
    public ushort port = 7777;

    public TMP_InputField ipInputField; 
    
    public void startHost()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipAddress, port);

        if (NetworkManager.Singleton.StartHost())
        {
            NetworkManager.Singleton.SceneManager.LoadScene(GamePlayScene, LoadSceneMode.Single);
        }
        //NetworkManager.Singleton.StartHost();
       
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        NetworkManager.Singleton.SceneManager.LoadScene(GamePlayScene, LoadSceneMode.Single);
    }
    
    public void StartClient()
    {
        string ConnectIp = ipInputField.text;
        
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ConnectIp, port);
        NetworkManager.Singleton.StartClient();
    }
}
