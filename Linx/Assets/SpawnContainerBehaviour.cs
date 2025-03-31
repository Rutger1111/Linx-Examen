using UnityEngine;
using Unity.Netcode;

public class SpawnContainerBehaviour : NetworkBehaviour
{
    public NetworkVariable<float> spawnCoolDown = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
}

