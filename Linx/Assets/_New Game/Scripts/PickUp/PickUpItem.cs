using Unity.Netcode;
using UnityEngine.Serialization;

public class PickUpItem : NetworkBehaviour
{
    public NetworkVariable<bool> isHeld = new NetworkVariable<bool>(false);
    
}
