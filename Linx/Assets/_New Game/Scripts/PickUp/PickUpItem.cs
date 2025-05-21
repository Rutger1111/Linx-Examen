using Unity.Netcode;
using UnityEngine;

public class PickUpItem : NetworkBehaviour
{
    public NetworkVariable<bool> IsHeld = new NetworkVariable<bool>(false);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
