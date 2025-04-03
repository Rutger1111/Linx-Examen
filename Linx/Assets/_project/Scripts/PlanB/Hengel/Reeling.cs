using UnityEngine;
using Unity.Netcode;

namespace _project.Scripts.PlanB
{
    public class Reeling : NetworkBehaviour
    {
        public GameObject _hook;
        public GameObject _fishingRod;

        //public GameObject hookposition;
        
        [SerializeField] private float lineLength;
        [SerializeField] private float distance;
        [SerializeField] public float resistance;

        public int weightOfFish;

        public float speed;

        private LineRenderer _lineRenderer;
        
        public ulong horizontalPlayerId = 0;
        public ulong verticalPlayerId = 1;
        
        private float maxSpeed; // Initial speed when the hook is near
        private float slowdownFactor; // Adjust this to fine-tune slowdown effect
        public float currentSpeed;

        private void Start()
        {
            //_hook = GameObject.Find("HookStartLine");
            _fishingRod = GameObject.Find("LineStartPoint");
            _lineRenderer = FindAnyObjectByType<LineRenderer>();

            if (IsServer)
            {
                horizontalPlayerId = NetworkManager.ConnectedClientsList[0].ClientId; 
                verticalPlayerId = NetworkManager.ConnectedClientsList[1].ClientId;
            }
        }

        void Update()
        {
            _lineRenderer.SetPosition(0, _fishingRod.transform.position);
            _lineRenderer.SetPosition(1, _hook.transform.position);

            MaxLineLength();
            ResistanceCalculation();

            Vector3 moveDirection = Vector3.zero;
            Quaternion rotateDirection = Quaternion.identity;

            ulong localClientId = NetworkManager.LocalClientId;
    
            maxSpeed = 10f;
            slowdownFactor = 0.05f;
            currentSpeed = maxSpeed * (1 / (1 + distance * slowdownFactor));

            
            if (localClientId == horizontalPlayerId)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    MoveToRod();
                    rotateDirection = Quaternion.AngleAxis(-10, Vector3.right);
                }

                if (Input.GetKey(KeyCode.S))
                {
                    moveDirection.y -= currentSpeed * Time.deltaTime;
                    rotateDirection = Quaternion.AngleAxis(180, -Vector3.right);
                }
            }

            if (moveDirection != Vector3.zero)
            {
                MoveHookServerRpc(moveDirection);
            }

            if (rotateDirection != Quaternion.identity)
            {
                RotatePlayerServerRpc(rotateDirection);
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void MoveHookServerRpc(Vector3 move, ServerRpcParams rpcParams = default)
        {
            _hook.transform.position += move; 
            MoveHookClientRpc(_hook.transform.position);
        }
        [ServerRpc(RequireOwnership = false)]
        public void RotatePlayerServerRpc(Quaternion rotate, ServerRpcParams rpcParams = default)
        {
            _hook.transform.rotation = rotate; 
            RotatePlayerClientRpc(_hook.transform.rotation);
        }
        
        [ClientRpc]
        private void MoveHookClientRpc(Vector3 newPos)
        {
            if (!IsOwner) _hook.transform.position = newPos;
        }
        [ClientRpc]
        private void RotatePlayerClientRpc(Quaternion newRot)
        {
            if (!IsOwner) _hook.transform.rotation = newRot;
        }
        
        public void MoveToRod()
        {
            _hook.transform.position = Vector3.Lerp(_hook.transform.position, _fishingRod.transform.position, 0.25f * Time.deltaTime);
            
        }

        private void MaxLineLength()
        {
            var rodPosition = _fishingRod.transform.position;
            distance = Vector3.Distance(_hook.transform.position, rodPosition);

            if (distance > lineLength)
            {
                
                Vector3 fromOriginToObject = _hook.transform.position - rodPosition;
                fromOriginToObject *= lineLength / distance;
                _hook.transform.position = rodPosition + fromOriginToObject;
            }
        }
        //doesn't work.
        protected void ResistanceCalculation()
        {
            resistance = distance + _hook.transform.position.y;
        }
        
        private void OnDrawGizmos()
        {
            // Set the color with custom alpha.
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f); // Red with custom alpha

            // Draw the sphere.
            Gizmos.DrawWireSphere(_hook.transform.position, 5);

            // Draw wire sphere outline.
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(_hook.transform.position, 10);
            
           
        }
    }
}
