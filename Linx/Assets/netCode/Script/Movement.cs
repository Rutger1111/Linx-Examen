using UnityEngine;
using Unity.Netcode;

public class Movement : NetworkBehaviour
{
    public float speed;
    
    void Update()
    {
        handleMovement();
    }

    void handleMovement()
    {
        float horizontal = 0f;
        float vertical = 0f;
            
        if (OwnerClientId == 0)
        {
            horizontal = Input.GetAxis("Horizontal");
        }
        else if (OwnerClientId == 1)
        {
            vertical = Input.GetAxis("Vertical");    
        }
        

        Vector3 movement = new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;

        transform.position += movement;
    }
}
