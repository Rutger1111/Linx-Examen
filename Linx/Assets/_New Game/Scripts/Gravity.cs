using UnityEngine;
using UnityEngine.Serialization;

public class Gravity : MonoBehaviour
{
    [SerializeField] private float _gravityForce = 20f;
    
    [SerializeField] private float groundCheckDistance = 0.1f; 
    
    [SerializeField] public bool hasGravity = true;
    
    [SerializeField] private bool isGrounded;

    [SerializeField] private LayerMask groundLayer;

    void Update()
    {
        CheckIfGrounded();
        
        if (hasGravity == true && isGrounded == false)
        {
            gravity();
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

    private void OnDrawGizmosSelected()
    {
        Vector3 start = transform.position;

        Vector3 end = transform.position + Vector3.down * groundCheckDistance;
        
        Gizmos.DrawLine(start, end);
    }
    
}
