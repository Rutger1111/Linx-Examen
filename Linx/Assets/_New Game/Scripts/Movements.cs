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

        [SerializeField] private float CenterMouseTimer = 0.4f;

        void Update()
        {
            if (CenterMouseTimer <= 0)
            {
                Screen.lockCursor = true;
            }
            
            CenterMouseTimer -= Time.deltaTime;
            
            if (IsOwner)
            {
                RotateBase();
                MoveArm();
                //MoveHook();
                Drive();
                Turn();
            }
        }
        
        private void Drive()
        {
            float driveInput = 0f;

            if (Input.GetKey(KeyCode.W)) driveInput = 1f;
            if (Input.GetKey(KeyCode.S)) driveInput = -1f;

            transform.position += wheelPivot.forward * (driveInput * driveSpeed * Time.deltaTime);
        }

        private void Turn()
        {
            float turnInput = 0f;

            if (Input.GetKey(KeyCode.D)) turnInput = 1f;
            if (Input.GetKey(KeyCode.A)) turnInput = -1f;
            
            wheelPivot.Rotate(0f, turnInput * baseRotationSpeed * Time.deltaTime, 0f);
        }

        private void RotateBase()
        {
            float horizontal = 0f;

            if (Input.GetKey(KeyCode.Mouse0)) horizontal = -1f;
            if (Input.GetKey(KeyCode.Mouse1)) horizontal = 1f;
            
            cranePivot.Rotate(0f, horizontal * baseRotationSpeed * Time.deltaTime, 0f);
        }

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

        private void MoveHook()
        {
            float hookInput = 0f;
            if (Input.GetKey(KeyCode.F)) hookInput = 1f;
            if (Input.GetKey(KeyCode.C)) hookInput = -1f;

            Vector3 localPos = craneHook.localPosition;
            localPos.y = Mathf.Clamp(localPos.y + hookInput * hookSpeed * Time.deltaTime, -maxHookHeight, -minHookHeight);
            craneHook.localPosition = localPos;
        }

        private void UpdateArmSupport()
        {
            Vector3 startPos = startSupport.position;
            Vector3 endPos = finishSupport.position;

            // Direction and distance between the points
            Vector3 direction = endPos - startPos;
            float distance = direction.magnitude;

            // Midpoint becomes the position of the arm
            Vector3 midPoint = startPos + (direction * 0.5f);
            supportArm.transform.position = midPoint;

            // Rotation to face from start to finish
            supportArm.transform.rotation = Quaternion.LookRotation(direction);

            // Scale the arm to match the distance (only along Z)
            Vector3 newScale = supportArm.transform.localScale;
            newScale.z = distance;
            supportArm.transform.localScale = newScale;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(startSupport.position, finishSupport.position);
        }
    }
}