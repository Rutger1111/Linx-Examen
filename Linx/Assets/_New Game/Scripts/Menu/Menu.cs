using System;
using System.Collections.Generic;
using _New_Game.Scripts.Crane;
using NUnit.Framework;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : NetworkBehaviour
{
    
    [SerializeField] private GameObject pausePanel;

    [SerializeField] private List<ThirdPersonCameraPlayerFollow> camerasList;
    [SerializeField] private List<Movement> movementList;

    [Header("Tutorial")] 
    
    private int index;
    
    [SerializeField] private List<GameObject> tutorialPanel = new List<GameObject>();
    [SerializeField] private GameObject tutorialPanel1;
    public bool TutoActive = false;
    public bool menuActive = true;

    private void Update()
    {
        InputHandler();
        FindingCamera();
        
        if (TutoActive == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            foreach (ThirdPersonCameraPlayerFollow Cameras in camerasList) 
            {
                Cameras.CameraDissable(menuActive);
            }
            foreach (Movement movement in movementList)
            {
                movement.MovementDisable(menuActive);
            }
        }
    }
    
    public void NextPanel(int direction)
    {
        if (index >= tutorialPanel.Count || index < 0)
        {
            index = 0;
            CloseTutorial();
        }
        else
        {
            tutorialPanel[index].SetActive(false);
            index += direction;
            tutorialPanel[index].SetActive(true);
        }

    }

    public void OpenTutorial()
    {
        tutorialPanel1.SetActive(true);
        TutoActive = false;
    }

    public void CloseTutorial()
    {
        tutorialPanel1.SetActive(false);
        TutoActive = true;
        menuActive = true;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void InputHandler()
    {
        menuActive = pausePanel.activeSelf != true;
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(menuActive);
        }
    }

    public void Continue()
    {
        pausePanel.SetActive(false);
        menuActive = true;
    }

    public void Retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void ToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }

    public void FindingCamera()
    {
        camerasList.Clear();

        ThirdPersonCameraPlayerFollow[] foundCameras = FindObjectsOfType<ThirdPersonCameraPlayerFollow>();

        foreach (ThirdPersonCameraPlayerFollow cam in foundCameras)
        {
            if (!camerasList.Contains(cam))
            {
                camerasList.Add(cam);
            }
        }
        
        movementList.Clear();

        Movement[] FoundMovement = FindObjectsOfType<Movement>();

        foreach (Movement mov in FoundMovement)
        {
            if (!movementList.Contains(mov))
            {
                movementList.Add(mov);
            }
        }
    }
}
