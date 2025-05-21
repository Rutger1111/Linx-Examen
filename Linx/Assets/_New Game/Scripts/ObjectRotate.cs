using UnityEngine;

namespace _New_Game.Scripts
{
    public class ObjectRotate : MonoBehaviour
    {
        [SerializeField] private GameObject hookPoint1;
        [SerializeField] private GameObject hookPoint2;
        [SerializeField] private GameObject box;

        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float followSpeed = 5f;

        void Update()
        {
            if (hookPoint1 == null || hookPoint2 == null || box == null)
            {
                Debug.LogWarning("Missing reference in ObjectRotate! Check hookPoint1, hookPoint2, or box.");
                return;
            }
        
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
}
