using Unity.Mathematics;
using UnityEngine;


public class HoverInMiddle : MonoBehaviour
{
    [SerializeField] private GameObject _hookPoint1;
    [SerializeField] private GameObject _hookPoint2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
            if (_hookPoint1 != null && _hookPoint2 != null)
            {
            if (_hookPoint1 == null || _hookPoint2 == null) return;

            // 1. Move to midpoint
            Vector3 midpoint = (_hookPoint1.transform.position + _hookPoint2.transform.position) / 2f;
            transform.position = midpoint;

            // 2. Calculate direction from point 1 to point 2
            Vector3 hookDirection = (_hookPoint2.transform.position - _hookPoint1.transform.position).normalized;
            if (hookDirection == Vector3.zero) return;

            // 3. Rotate object's UP axis to face the hook direction
            Quaternion rotationToHook = Quaternion.FromToRotation(Vector3.left, hookDirection);
            rotationToHook.eulerAngles = new Vector3(rotationToHook.eulerAngles.x -90, rotationToHook.eulerAngles.y + 90, rotationToHook.eulerAngles.z + 90);
            // 4. Apply +90° rotation around the Y-axis
            Quaternion yOffset = Quaternion.Euler(0, 90f, 0);
            transform.rotation = rotationToHook * yOffset;
        }
        else
        {
            Debug.LogWarning("Please assign both GameObjects.");
        }
    }
}
