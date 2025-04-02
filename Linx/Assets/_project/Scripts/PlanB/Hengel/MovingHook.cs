using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;


namespace _project.Scripts.PlanB
{
    public class MovingHook : NetworkBehaviour
    {
        public Reeling _reeling;
        private Vector3 moveDirection;
        
        void Update()
        {
            moveDirection = Vector3.zero;
            ulong localClientId = NetworkManager.LocalClientId;
            
            if (localClientId == _reeling.verticalPlayerId)
            {
                if (Input.GetKey(KeyCode.A)) moveDirection.x -= 10 * Time.deltaTime;
                if (Input.GetKey(KeyCode.D)) moveDirection.x += 10 * Time.deltaTime;

                //ResistanceCalculation();
            }

            if (moveDirection != Vector3.zero)
            {
                _reeling.MoveHookServerRpc(moveDirection);
            }
        }
    }
}
