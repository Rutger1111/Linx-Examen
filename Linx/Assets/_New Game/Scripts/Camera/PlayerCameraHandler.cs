using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace _New_Game.Scripts.Camera
{
    public class PlayerCameraHandler : NetworkBehaviour
    {
        [SerializeField] private UnityEngine.Camera usedCamera;
    
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        
            StartCoroutine(WaitAndEnableCamera());
        }

        private IEnumerator WaitAndEnableCamera()
        {
            yield return new WaitForEndOfFrame(); 

            if (!IsOwner)
            {
                yield break;
            }

        
            usedCamera.enabled = true;
        }
    }
}

