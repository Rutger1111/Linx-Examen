using System;
using System.Collections.Generic;
using _New_Game.Scripts.Crane;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    
    [SerializeField] private GameObject pausePanel;

    [SerializeField] private List<ThirdPersonCameraPlayerFollow> camerasList;
    [SerializeField] private List<Movement> movementList;

    [Header("Tutorial")] 
    
    private int index;
    
    [SerializeField] private List<GameObject> tutorialPanel = new List<GameObject>();
    [SerializeField] private GameObject tutorialPanel1;
    private bool TutoActive = true;
    public bool menuActive = false;
    private void Start()
    {
        
    }

    private void Update()
    {

        InputHandler();
        
        FindingCamera();
        
        if (TutoActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = TutoActive;
            
            
            foreach (ThirdPersonCameraPlayerFollow Cameras in camerasList)
            {
                Cameras.CameraDissable(TutoActive);
            }
            
            foreach (Movement movement in movementList)
            {
                movement.MovementDisable(TutoActive);
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
            index += 1;
            tutorialPanel[index].SetActive(true);
        }

    }

    public void OpenTutorial()
    {
        tutorialPanel1.SetActive(true);
        TutoActive = true;
    }

    public void CloseTutorial()
    {
        tutorialPanel1.SetActive(false);
        TutoActive = false;
        
        foreach (ThirdPersonCameraPlayerFollow Cameras in camerasList)
        {
            Cameras.CameraDissable(menuActive);
        }
        foreach (Movement movement in movementList)
        {
            movement.MovementDisable(menuActive);
        }
    }

    private void InputHandler()
    {
        bool menuActive = pausePanel.activeSelf != true;
        
        
        menuActive = pausePanel.activeSelf != true;
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print(menuActive);
            
            pausePanel.SetActive(menuActive);
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
