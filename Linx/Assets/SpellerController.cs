using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class SpellerController : NetworkBehaviour
{
    public NetworkObject Redhook;
    public NetworkObject BlueHook;
    
    void Start()
    {
        var clientIds = NetworkManager.Singleton.ConnectedClientsIds.ToList();
        
        Redhook.ChangeOwnership(clientIds[1]);
        BlueHook.ChangeOwnership(clientIds[2]);
    }

   
    void Update()
    {
       
    }
}
