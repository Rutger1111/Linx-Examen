using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraPlayerFollow : MonoBehaviour
{
    public GameObject player;
    
    public Vector3 offset = new Vector3(0,3,-6);

    public float sensitivity = 4f;
    public float distance = 6f;
    public float yMin = -20f;
    public float yMax = 80f;

    private float currentX = 0f;
    private float currentY = 0f;
    
    void Update()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity;
        currentY = Mathf.Clamp(currentY, yMin, yMax);
        
        rotationFollow();
    }

    public void rotationFollow()
    {
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 disiredPosition = player.transform.position + rotation * new Vector3(0, 0 - distance);
        transform.position = disiredPosition + new Vector3(0, offset.y, 0);
        transform.LookAt(player.transform.position + Vector3.up * offset.y);

    }
}
