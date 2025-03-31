using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    public float speed;

    public float distanceForMovement;
    
    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;

        if (Mathf.Abs(direction.x) > distanceForMovement || Mathf.Abs(direction.y) > distanceForMovement)
        {
            Vector3 newPosition = transform.position + new Vector3(direction.x, direction.y, 0).normalized * speed * Time.deltaTime;
        
        
            newPosition.z = transform.position.z;

            transform.position = newPosition;    
            
        }

        
    }
}
