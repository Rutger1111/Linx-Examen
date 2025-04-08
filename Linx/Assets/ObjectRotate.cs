using Unity.Mathematics;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    public GameObject hookPoint1, hookPoint2;

    public GameObject box;

    public float rotationSpeed = 5f;
    public float followSpeed = 5f;
    void Start()
    {
        
    }

    
    void Update()
    {
        FollowHook();
        RotateHook();
    }

    public void FollowHook()
    {
        Vector3 midpoint = (hookPoint1.transform.position + hookPoint2.transform.position) / 2f;
        
        box.transform.position = Vector3.Lerp(box.transform.position, midpoint, followSpeed * Time.deltaTime);
    }
    
    public void RotateHook()
    {
        float yDiff = hookPoint2.transform.position.y - hookPoint1.transform.position.y;

        float targetAngle = yDiff * 10f;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, -targetAngle);
        
        box.transform.rotation =
            Quaternion.Slerp(box.transform.rotation, targetRotation,rotationSpeed * Time.deltaTime);
    }
}
