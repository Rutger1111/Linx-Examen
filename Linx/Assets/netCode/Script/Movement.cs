using UnityEngine;


public class Movement : MonoBehaviour
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
            
        
            horizontal = Input.GetAxis("Horizontal");
        
            vertical = Input.GetAxis("Vertical");    
        
        

        Vector3 movement = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;

        transform.position += movement;
    }
}
