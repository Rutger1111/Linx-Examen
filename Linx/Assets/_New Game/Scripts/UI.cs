using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : NetworkBehaviour
{
    [SerializeField] private GameObject PauseScreen;
    

    public void resumeGame()
    {
        PauseScreen.SetActive(false);
    }

    public void QuitGame()
    {
        NetworkManager.Singleton.Shutdown();
        NetworkManager.SceneManager.LoadScene("Lobby", LoadSceneMode.Additive);
    }
}
