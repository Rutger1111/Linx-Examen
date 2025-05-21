using Unity.Netcode;
using UnityEngine;

namespace _New_Game.Scripts
{
    public class PlayerInfo : NetworkBehaviour
    {
        public ulong OwnerClientId { get; private set; }

        public void SetClientId(ulong id)
        {
            OwnerClientId = id;
        }

        public override void OnNetworkSpawn()
        {
            OwnerClientId = OwnerClientId; // Automatically handled by Netcode
            Debug.Log($"[PlayerInfo] Player spawned with client ID: {OwnerClientId}");
        }
    }
}
