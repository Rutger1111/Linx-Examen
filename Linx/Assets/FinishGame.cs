using System.Collections.Generic;
using _New_Game.Scripts.Camera;
using _New_Game.Scripts.Crane;
using _New_Game.Scripts.Menu;
using _New_Game.Scripts.Snapping;
using Unity.Netcode;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class FinishGame : NetworkBehaviour
{

    [SerializeField] private List<Snap> _snaps = new List<Snap>();

    [SerializeField] private GameObject _winUI;

    [SerializeField] private Menu _menu;
    
    [SerializeField] private List<ThirdPersonCameraPlayerFollow> camerasList;
    [SerializeField] private List<Movement> movementList;

    
    void Update()
    {
        FindingCamera();
        
        if (_snaps.TrueForAll(snap => snap.blockPlaced))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            _winUI.SetActive(true);
            
            _menu.enabled = false;
            
            foreach (ThirdPersonCameraPlayerFollow Cameras in camerasList) 
            {
                Cameras.CameraDissable(false);
            }
            foreach (Movement movement in movementList)
            {
                movement.MovementDisable(false);
            }
        }
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
