using Unity.Netcode;
using UnityEngine;

public class PickUpItem : NetworkBehaviour
{
    public NetworkVariable<bool> IsHeld = new NetworkVariable<bool>(false);
    public GameObject Wall;
    
}
