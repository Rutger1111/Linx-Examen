using System;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.Netcode;

namespace _New_Game.Scripts.Crane
{
    public class Movement : NetworkBehaviour
    {
        [SerializeField] private GameObject supportArm;
        
        [Header("Transforms")]
        [SerializeField] private Transform cranePivot;
        [SerializeField] private Transform craneArm;
        [SerializeField] private Transform craneHook;
        [SerializeField] private Transform wheelPivot;
        [SerializeField] private Transform startSupport;
        [SerializeField] private Transform finishSupport;

        [Header("Speeds")]
        [SerializeField] private float baseRotationSpeed = 50f;
        [SerializeField] private float driveSpeed = 5f;
        [SerializeField] private float armRotationSpeed = 30f;
        [SerializeField] private float hookSpeed = 2f;

        [Header("craneArm angle")]
        [SerializeField] private float minArmAngle = -45f;
        [SerializeField] private float maxArmAngle = 1f;

        [Header("hook settings")]
        [SerializeField] private float minHookHeight = 0.5f;
        [SerializeField] private float maxHookHeight = 10f;

        public bool hasMovementOptions;
        private bool isGrounded;

        public AudioSource driving;
        public AudioSource grab;

        void Update()
        {
            if (IsOwner)
            {
                //RotateBase();
                //MoveArm();
                //MoveHook();
                
                if (hasMovementOptions)
                {
                    StretchBetweenPoints(supportArm.transform, startSupport, finishSupport);
                    Drive();
                    Turn();
                    Grab();
                }
            }
        }


        public void MovementDisable(bool CanMove)
        {
            hasMovementOptions = CanMove;
            
        }
        private void Drive()
        {
            float driveInput = 0f;

            if (Input.GetKey(KeyCode.W))
            {
                driveInput = 1f;
                if (!driving.isPlaying)
                {
                    driving.Play();    
                }
            }
            else
            {
                driving.Stop();
            }
            
            if (Input.GetKey(KeyCode.S))
            {
                driveInput = -1f;
                driving.Play();    
                
            }
            else
            {
                driving.Stop();
            }

            transform.position += wheelPivot.forward * (driveInput * driveSpeed * Time.deltaTime);
            
            driving.Play();
        }

        private void Turn()
        {
            
            float turnInput = 0f;

            if (Input.GetKey(KeyCode.D))
            {
                turnInput = 1f;
                driving.Play();    
                
            }
            else
            {
                driving.Stop();
            }

            if (Input.GetKey(KeyCode.A))
            {
                turnInput = -1f;
                if (!driving.isPlaying)
                {
                    driving.Play();    
                }
            }
            else
            {
                driving.Stop();
            }
            
            wheelPivot.Rotate(0f, turnInput * baseRotationSpeed * Time.deltaTime, 0f);
        }

        /*private void RotateBase()
        {
            //float horizontal = 0f;
            float horizontal = _craneMovement.Driving.TurnBase.ReadValue<float>();

            //if (Input.GetKey(KeyCode.Mouse0)) horizontal = -1f;
            //if (Input.GetKey(KeyCode.Mouse1)) horizontal = 1f;
            
            cranePivot.Rotate(0f, horizontal * baseRotationSpeed * Time.deltaTime, 0f);
        }*/

        private void MoveArm()
        {
            float armInput = 0f;

            if (Input.mouseScrollDelta.y >= 0.1) armInput = 4f;
            if (Input.mouseScrollDelta.y <= -0.1) armInput = -4f;

            float currentX = craneArm.localEulerAngles.x;
            if (currentX > 180f) currentX -= 360f;

            float newX = Mathf.Clamp(currentX - armInput * armRotationSpeed * Time.deltaTime, -maxArmAngle, -minArmAngle);
            craneArm.localEulerAngles = new Vector3(newX, 0f, 0f);
        }

        private void Grab()
        {
            // Initialize grab input value (used to determine if grab is active)
            float grabInput = 0f;

            // Check if the left mouse button is being held down
            if (Input.GetKey(KeyCode.Mouse0))
            {
                grabInput = 1f;
                grab.Play();
            }
            else
            {
                grab.Stop();
            }

            // Get the current rotation of the crane arm
            Vector3 currentAngles = craneArm.localEulerAngles;

            // Normalize the X angle to the range [-180, 180] to avoid issues with rotation wrapping
            float currentX = currentAngles.x;
            if (currentX > 180) currentX -= 360;

            float speed = 20f; // Speed at which the crane arm moves
            float newX = currentX; // Store current X angle to modify it

            // If grabbing, rotate arm upwards; otherwise, rotate it downwards
            if (grabInput > 0)
            {
                newX += speed * Time.deltaTime;
            }
            else
            {
                newX -= speed * Time.deltaTime;
            }

            // Clamp the new X angle to stay within defined min and max angles
            newX = Mathf.Clamp(newX, minArmAngle, maxArmAngle);

            // Apply the new x rotation to the crane arm 
            craneArm.localEulerAngles = new Vector3(newX, currentAngles.y, currentAngles.z);
        }

        private void MoveHook()
        {
            float hookInput = 0f;
            if (Input.GetKey(KeyCode.F)) hookInput = 1f;
            if (Input.GetKey(KeyCode.C)) hookInput = -1f;

            Vector3 localPos = craneHook.localPosition;
            localPos.y = Mathf.Clamp(localPos.y + hookInput * hookSpeed * Time.deltaTime, -maxHookHeight, -minHookHeight);
            craneHook.localPosition = localPos;
        }

        public void StretchBetweenPoints(Transform obj, Transform startPoint, Transform endPoint)
        {
            Vector3 startPos = startPoint.position;
            Vector3 endPos = endPoint.position;

            // Direction and distance
            Vector3 direction = endPos - startPos;
            float distance = direction.magnitude * 0.1f;

            // Move to midpoint
            obj.position = startPos + direction * 0.5f;

            // Rotate to face the target
            obj.rotation = Quaternion.LookRotation(direction);
            
            // stretch from start to finish along the z-as
            Vector3 newScale = obj.localScale;
            newScale.z = distance;
            obj.localScale = newScale;
            
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(startSupport.position, finishSupport.position);
        }
    }
}