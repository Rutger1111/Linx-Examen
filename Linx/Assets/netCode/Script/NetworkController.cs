using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine.UI;
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
            NetworkManager.Singleton.SceneManager.LoadScene(_gameplayScene, LoadSceneMode.Single);
        });
        _clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
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
