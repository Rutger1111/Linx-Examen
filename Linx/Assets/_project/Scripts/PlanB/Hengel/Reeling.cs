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
            ulong localClientId = NetworkManager.LocalClientId;
            
            if (localClientId == horizontalPlayerId)
            {
                if (Input.GetKey(KeyCode.W)) MoveToRod();
                if (Input.GetKey(KeyCode.S)) moveDirection.y -= 7 * Time.deltaTime;
            }
            /*else if (localClientId == verticalPlayerId)
            {
                if (Input.GetKey(KeyCode.A)) moveDirection.x -= 10 * Time.deltaTime;
                if (Input.GetKey(KeyCode.D)) moveDirection.x += 10 * Time.deltaTime;

                ResistanceCalculation();
            }*/

            if (moveDirection != Vector3.zero)
            {
                MoveHookServerRpc(moveDirection);
            }
            
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void MoveHookServerRpc(Vector3 move, ServerRpcParams rpcParams = default)
        {
            _hook.transform.position += move; 
            MoveHookClientRpc(_hook.transform.position);
        }
        
        [ClientRpc]
        private void MoveHookClientRpc(Vector3 newPos)
        {
            if (!IsOwner) _hook.transform.position = newPos;
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
