using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace _New_Game.Scripts
{
    public class Movements : NetworkBehaviour
    {
        [Header("Transforms")]
        [SerializeField] private Transform cranePivot;
        [SerializeField] private Transform craneArm;
        [SerializeField] private Transform craneHook;
        [SerializeField] private Transform wheelPivot;

        [Header("Speeds")]
        [SerializeField] private float baseRotationSpeed = 50f;
        [SerializeField] private float armRotationSpeed = 30f;
        [SerializeField] private float hookSpeed = 2f;
        [SerializeField] private float driveSpeed = 20f;

        [Header("craneArm angle")]
        [SerializeField] private float minArmAngle = -45f;
        [SerializeField] private float maxArmAngle = 1f;

        [Header("hook settings")]
        [SerializeField] private float minHookHeight = 0.5f;
        [SerializeField] private float maxHookHeight = 10f;

        

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                enabled = false;
                return;
            }
        }
        void Update()
        {
            if (IsOwner)
            {
                RotateBase();
                MoveArm();
                MoveHook();
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

            if (Input.GetKey(KeyCode.LeftArrow)) horizontal = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) horizontal = 1f;
            
            cranePivot.Rotate(0f, horizontal * baseRotationSpeed * Time.deltaTime, 0f);
        }

        private void MoveArm()
        {
            float armInput = 0f;

            if (Input.GetKey(KeyCode.F)) armInput = 1f;
            if (Input.GetKey(KeyCode.C)) armInput = -1f;

            float currentX = craneArm.localEulerAngles.x;
            if (currentX > 180f) currentX -= 360f;

            float newX = Mathf.Clamp(currentX - armInput * armRotationSpeed * Time.deltaTime, -maxArmAngle, -minArmAngle);
            craneArm.localEulerAngles = new Vector3(newX, 0f, 0f);
        }

        private void MoveHook()
        {
            float hookInput = 0f;
            if (Input.GetKey(KeyCode.G)) hookInput = 1f;
            if (Input.GetKey(KeyCode.V)) hookInput = -1f;

            Vector3 localPos = craneHook.localPosition;
            localPos.y = Mathf.Clamp(localPos.y + hookInput * hookSpeed * Time.deltaTime, -maxHookHeight, -minHookHeight);
            craneHook.localPosition = localPos;
        }

    }
}