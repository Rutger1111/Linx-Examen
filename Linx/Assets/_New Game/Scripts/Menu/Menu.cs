using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : NetworkBehaviour
{
    [SerializeField] private GameObject pausePanel;
    
    
    private void Update()
    {
        InputHandler();
    }

    private void InputHandler()
    {
        bool menuActive = pausePanel.activeSelf != true;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(menuActive);
        }
    }

    public void Continue()
    {
        pausePanel.SetActive(false);
    }

    public void Retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void ToMainMenu()
    {
        NetworkManager.Singleton.Shutdown();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }
}
