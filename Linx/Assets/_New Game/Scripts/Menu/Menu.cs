using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    private void Update()
    {
        InputHandler();
    }

    private void InputHandler()
    {
        bool hoi = pausePanel.activeSelf == true ? false : true;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(hoi);
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
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }
}
