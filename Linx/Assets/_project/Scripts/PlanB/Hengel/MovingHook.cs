using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;


namespace _project.Scripts.PlanB
{
    public class MovingHook : NetworkBehaviour
    {
        public ulong horizontalPlayerId = 0;
        public ulong verticalPlayerId = 1;

        private void Start()
        {
            if (IsServer)
            {
                horizontalPlayerId = NetworkManager.ConnectedClientsList[0].ClientId; 
                verticalPlayerId = NetworkManager.ConnectedClientsList[1].ClientId;
            }
        }
        void Update()
        {//player 1 should move these
            Vector3 moveDirection = Vector3.zero;
            ulong localClientId = NetworkManager.LocalClientId;
            
            if (localClientId == verticalPlayerId)
            {
                if (Input.GetKey(KeyCode.A)) moveDirection.x -= 10 * Time.deltaTime;
                if (Input.GetKey(KeyCode.D)) moveDirection.x += 10 * Time.deltaTime;

                //ResistanceCalculation();
            }
            Range();
            
            if (moveDirection != Vector3.zero)
            {
                MoveHookServerRpc(moveDirection);
            }
        }
        
        [ServerRpc(RequireOwnership = true)]
        private void MoveHookServerRpc(Vector3 move, ServerRpcParams rpcParams = default)
        {
            transform.position += move; 
            MoveHookClientRpc(transform.position);
        }
        
        [ClientRpc]
        private void MoveHookClientRpc(Vector3 newPos)
        {
            if (!IsOwner) transform.position = newPos;
        }
        

        private void Range()
        {
            Gizmos.DrawSphere(gameObject.transform.position, 5f);
        }
        
        private void OnDrawGizmos()
        {
            // Set the color with custom alpha.
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f); // Red with custom alpha

            // Draw the sphere.
            Gizmos.DrawWireSphere(transform.position, 5);

            // Draw wire sphere outline.
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, 5);
        }
        
        private void Catch()
        {
            
        }
    }
}
