using FishSystem;
using PlanB;
using Unity.Netcode;
using UnityEngine;

namespace _project.Scripts.PlanB.Hengel
{
    public class MovingHook : NetworkBehaviour
    {
        public Reeling _reeling;
        public Vector3 moveDirection;
        public Quaternion rotateDirection;
        
        void Update()
        {
            moveDirection = Vector3.zero;
            rotateDirection = Quaternion.identity; // Use Quaternion.identity instead of new Quaternion(0,0,0,0)
            ulong localClientId = NetworkManager.LocalClientId;
            
            if (localClientId == _reeling.verticalPlayerId)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    moveDirection.x -= 10 * Time.deltaTime;
                    rotateDirection = Quaternion.Euler(0, 0, 90); // Proper way to set rotation
                }

                if (Input.GetKey(KeyCode.D))
                {
                    moveDirection.x += 10 * Time.deltaTime;
                    rotateDirection = Quaternion.Euler(0, 0, -90); // Proper way to set rotation
                }
            }

            if (moveDirection != Vector3.zero)
            {
                _reeling.MoveHookServerRpc(moveDirection);
            }
            
            if (rotateDirection != Quaternion.identity) // Correct way to check for rotation change
            {
                RotatePlayerServerRpc(rotateDirection);
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void RotatePlayerServerRpc(Quaternion rotate, ServerRpcParams rpcParams = default)
        {
            transform.rotation = rotate; 
            RotatePlayerClientRpc(transform.rotation);
        }
        
        [ClientRpc]
        private void RotatePlayerClientRpc(Quaternion newRot)
        {
            if (!IsOwner) transform.rotation = newRot;
        }

        private void OnTriggerStay (Collider other)
        {
            if (Input.GetKeyDown(KeyCode.C) && other.CompareTag("Fish"))
            {
                gameObject.GetComponent<Catch>().Invoke(other.gameObject.GetComponent<Fish>());
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }
}
