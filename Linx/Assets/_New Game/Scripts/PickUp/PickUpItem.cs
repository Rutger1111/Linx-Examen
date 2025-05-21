using Unity.Netcode;
using UnityEngine.Serialization;

public class PickUpItem : NetworkBehaviour
{
    public NetworkVariable<bool> IsHeld = new NetworkVariable<bool>(false);
    
    void Start()
    {
        
    }

  
      void Update()
    {
        
    }
}
