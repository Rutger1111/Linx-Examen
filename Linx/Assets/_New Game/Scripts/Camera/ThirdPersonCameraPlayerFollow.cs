using Unity.Netcode;
using UnityEngine;

namespace _New_Game.Scripts.Camera
{
    public class ThirdPersonCameraPlayerFollow : NetworkBehaviour
    {
        [SerializeField] private GameObject player;

        [SerializeField] private Vector3 offset = new Vector3(0,3,-6);

        [SerializeField] private float sensitivity = 4f;
        [SerializeField] private float distance = 6f;
        [SerializeField] private float yMin = -20f;
        [SerializeField] private float yMax = 80f;

        private float _currentX;
        private float _currentY;

        private bool _hasTurnedOffCamera;
        private void Awake()
        {
            if (IsOwner)
            {
                NetworkManager.Singleton.OnClientStarted += OnClientStarted;
            }
        }

        private void OnClientStarted()
        {
        
            if (IsOwner && NetworkManager.Singleton.LocalClient.PlayerObject != null)
            {
                player = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;

            
                transform.SetParent(player.transform);
            }
        }
    
<<<<<<< HEAD
        void Update()
        {
            if (_hasTurnedOffCamera)
            {
                Cursor.lockState = CursorLockMode.Locked;
=======
    void Update()
    {
        if (hasTurnedOffCamera == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
>>>>>>> backup
            
                _currentX += Input.GetAxis("Mouse X") * sensitivity;
                _currentY -= Input.GetAxis("Mouse Y") * sensitivity;
                _currentY = Mathf.Clamp(_currentY, yMin, yMax);
        
                RotationFollow();    
            }
        }

        private void RotationFollow()
        {
            Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0);
            var position = player.transform.position;
            Vector3 disiredPosition = position + rotation * new Vector3(0, 0 - distance);
            transform.position = disiredPosition + new Vector3(0, offset.y, 0);
            transform.LookAt(position + Vector3.up * offset.y);
        }


        public void CameraDissable(bool disable)
        {
            _hasTurnedOffCamera = disable;
        }
    }
}
