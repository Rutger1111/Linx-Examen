using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkDisconnectHandler : MonoBehaviour
{
    void OnEnable()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    void OnDisable()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
