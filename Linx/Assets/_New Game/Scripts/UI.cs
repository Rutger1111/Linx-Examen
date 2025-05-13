using Unity.Netcode;
using UnityEngine;

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
    }
}
