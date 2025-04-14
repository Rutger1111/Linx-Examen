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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("Lobby");
    }
}
