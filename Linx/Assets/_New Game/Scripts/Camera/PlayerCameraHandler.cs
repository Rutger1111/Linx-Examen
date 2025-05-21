using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerCameraHandler : NetworkBehaviour
{
    [SerializeField] private Camera _camera;
    
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
            
            Debug.Log("Not Local Player, skipping camera enable.");
            yield break;
        }

        
        _camera.enabled = true;
        
        Debug.Log("Local Player: Camera enabled.");
    }
}

