using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class ThirdPersonCameraPlayerFollow : NetworkBehaviour
{
    [SerializeField] private GameObject _player;

    [SerializeField] private Vector3 _offset = new Vector3(0,3,-6);

    [SerializeField] private float _sensitivity = 4f;
    [SerializeField] private float _distance = 6f;
    [SerializeField] private float _yMin = -20f;
    [SerializeField] private float _yMax = 80f;

    private float currentX = 0f;
    private float currentY = 0f;

    private bool hasTurnedOffCamera = false;
    private void Awake()
    {
        if (IsOwner)
        {
            Unity.Netcode.NetworkManager.Singleton.OnClientStarted += OnClientStarted;
        }
    }

    private void OnClientStarted()
    {
        
        if (IsOwner && Unity.Netcode.NetworkManager.Singleton.LocalClient.PlayerObject != null)
        {
            _player = Unity.Netcode.NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;

            
            transform.SetParent(_player.transform);
        }
    }
    
    void Update()
    {
        if (hasTurnedOffCamera == false)
        {
            Cursor.lockState = CursorLockMode.None;
            
            currentX += Input.GetAxis("Mouse X") * _sensitivity;
            currentY -= Input.GetAxis("Mouse Y") * _sensitivity;
            currentY = Mathf.Clamp(currentY, _yMin, _yMax);
        
            rotationFollow();    
        }
    }

    public void rotationFollow()
    {
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 disiredPosition = _player.transform.position + rotation * new Vector3(0, 0 - _distance);
        transform.position = disiredPosition + new Vector3(0, _offset.y, 0);
        transform.LookAt(_player.transform.position + Vector3.up * _offset.y);
    }


    public void CameraDissable(bool disable)
    {
        hasTurnedOffCamera = disable;
    }
}
