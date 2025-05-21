using Unity.Netcode;
using UnityEngine;

namespace _New_Game.Scripts
{
    public class Movement : NetworkBehaviour
    {
        [SerializeField] private GameObject supportArm;
        
        [Header("Transforms")]
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
                
                if (hasMovementOptions == true)
                {
                    StretchBetweenPoints(supportArm.transform, startSupport, finishSupport);
                    Drive();
                    Turn();
                    Grab();
                }
            }
        }


        public void MovementDisable(bool canMove)
        {
            hasMovementOptions = canMove;
            
        }
        private void Drive()
        {
            float driveInput = 0f;
            //float driveInput = _craneMovement.Driving.drive.ReadValue<float>();

            if (Input.GetKey(KeyCode.W))
            {
                driveInput = 1f;
            }

            if (Input.GetKey(KeyCode.S))
            {
                driveInput = -1f;
            }

            transform.position += wheelPivot.forward * (driveInput * driveSpeed * Time.deltaTime);
            
            
            if (driveInput != 0f)
            {
                if (!driving.isPlaying)
                {
                    driving.Play();
                }
            }
            else
            {
                if (driving.isPlaying)
                {
                    driving.Stop();
                }
            }
            
        }

        private void Turn()
        {
            
            float turnInput = 0f;
            //float turnInput = _craneMovement.Driving.TurnWheels.ReadValue<float>();

            if (Input.GetKey(KeyCode.D))
            {
                turnInput = 1f;
            }
            

            if (Input.GetKey(KeyCode.A))
            {
                turnInput = -1f;
            }
            
            wheelPivot.Rotate(0f, turnInput * baseRotationSpeed * Time.deltaTime, 0f);
            
            if (turnInput != 0f)
            {
                if (!driving.isPlaying)
                {
                    driving.Play();
                }
            }
            else
            {
                if (driving.isPlaying)
                {
                    driving.Stop();
                }
            }
            
            
            
            
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
            float grabInput = 0f;

            if (Input.GetKey(KeyCode.Mouse0))
            {
                grabInput = 1f;
            }
            //Up(KeyCode.Mouse0)) grabInput = -1f;

            Vector3 currentAngles = craneArm.localEulerAngles;

            float currentX = currentAngles.x;
            if (currentX > 180) currentX -= 360;

            float speed = 20f;

            float newX = currentX;

            if (grabInput > 0)
            {
                newX += speed * Time.deltaTime;
            }
            else
            {
                newX -= speed * Time.deltaTime;
            }
            newX = Mathf.Clamp(newX, minArmAngle, maxArmAngle);

            craneArm.localEulerAngles = new Vector3(newX, currentAngles.y, currentAngles.z);

            if (grabInput != 0f)
            {
                if (!driving.isPlaying)
                {
                    grab.Play();
                }
            }
            else
            {
                if (driving.isPlaying)
                {
                    grab.Stop();
                }
            }
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

        private void StretchBetweenPoints(Transform obj, Transform startPoint, Transform endPoint)
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

            // Stretch along Z (assuming original length is 1 unit)
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