using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class NetWorkController : NetworkBehaviour
{
    public NetworkObject Redhook;
    public NetworkObject BlueHook;

    void Start()
    {
        if (!IsServer) return;
        
        var clientIds = NetworkManager.Singleton.ConnectedClientsIds.OrderBy(id => id).ToList();

        if (clientIds.Count >= 1)
        {
            Redhook.ChangeOwnership(clientIds[0]); 
        }

        if (clientIds.Count >= 2)
        {
            BlueHook.ChangeOwnership(clientIds[1]); 
        }
    }
}


