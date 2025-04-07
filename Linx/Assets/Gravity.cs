using UnityEngine;
using UnityEngine.Serialization;

public class Gravity : MonoBehaviour
{
    public float _gravityForce = 20f;

    public bool hasGravity = true;

    public bool isGrounded;
    
    public float groundCheckDistance = 0.1f; 
    public LayerMask groundLayer;
    void Update()
    {
        CheckIfGrounded();
        
        if (hasGravity == true && isGrounded == false)
        {
            gravity();
        }
        else
        {
            
        }
    }

    public void gravity()
    {
        transform.position += Vector3.down * _gravityForce * Time.deltaTime;
    }
    
    void CheckIfGrounded()
    {
        
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
