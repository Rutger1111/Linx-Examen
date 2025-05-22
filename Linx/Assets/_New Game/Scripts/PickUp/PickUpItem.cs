using Unity.Netcode;

public class PickUpItem : NetworkBehaviour
{
    public NetworkVariable<bool> IsHeld = new NetworkVariable<bool>(false);
    
}
