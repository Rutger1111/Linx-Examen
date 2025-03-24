using System;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    void handlemovement()
    {
        if (isLocalPlayer)
        {
            float movementhor = Input.GetAxis("Horizontal");
            float movementver = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(movementhor * 0.1f, movementver * 0.1f, 0);

            transform.position = transform.position + movement;
        }
    }

    private void Update()
    {
        handlemovement();
    }
}
