using System.Collections.Generic;
using _New_Game.Scripts.Camera;
using _New_Game.Scripts.Crane;
using Unity.Netcode;
using UnityEngine;

namespace _New_Game.Scripts.Menu
{
    public class Menu : NetworkBehaviour
    {
    
        [SerializeField] private GameObject pausePanel;

        [SerializeField] private List<ThirdPersonCameraPlayerFollow> camerasList;
        [SerializeField] private List<Movement> movementList;

        [Header("Tutorial")] 
    
        private int _index;
    
        [SerializeField] private List<GameObject> tutorialPanel;
        [SerializeField] private GameObject tutorialPanel1;
        private bool _tutoActive;
        public bool menuActive = true;

        private void Update()
        {

            if (!IsOwner)return;
        
        
            InputHandler();
            FindingCamera();
        
            if (_tutoActive == false)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                foreach (ThirdPersonCameraPlayerFollow cameras in camerasList) 
                {
                    cameras.CameraDissable(menuActive);
                }
                foreach (Movement movement in movementList)
                {
                    movement.MovementDisable(menuActive);
                }
            }
        }
    
        public void NextPanel(int direction)
        {
            if (_index >= tutorialPanel.Count || _index < 0)
            {
                _index = 0;
                CloseTutorial();
            }
            else
            {
                tutorialPanel[_index].SetActive(false);
                _index += direction;
                tutorialPanel[_index].SetActive(true);
            }

        }

        public void OpenTutorial()
        {
            tutorialPanel1.SetActive(true);
            _tutoActive = false;
        }

        public void CloseTutorial()
        {
            tutorialPanel1.SetActive(false);
            _tutoActive = true;
        
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

            Movement[] foundMovement = FindObjectsOfType<Movement>();

            foreach (Movement mov in foundMovement)
            {
                if (!movementList.Contains(mov))
                {
                    movementList.Add(mov);
                }
            }
        }
    }
}
