using UnityEngine;

namespace _New_Game.Scripts
{
    public class Gravity : MonoBehaviour
    {
        [SerializeField] private float gravityForce = 20f;
    
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
            transform.position += Vector3.down * (gravityForce * Time.deltaTime);
        }

        public void Rotation()
        {
            Vector3 euler = transform.rotation.eulerAngles;

            float xRot = NormalizeAngle(euler.x);
            float yRot = NormalizeAngle(euler.y);

            bool needsCorrection = xRot !> 90f || xRot !< -90f || yRot !> 90f || yRot !< -90f;

            if (needsCorrection)
            {
                Quaternion targetRotation = Quaternion.Euler(90f, 90f, 0);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100f * Time.deltaTime);
            }
        }

        float NormalizeAngle(float angle)
        {
            if (angle > 180f)
                angle -= 360f;
            return angle;
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
}
