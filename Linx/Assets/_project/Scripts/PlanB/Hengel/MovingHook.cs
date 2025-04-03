using System;
using FishSystem;
using PlanB;
using Unity.Netcode;
using UnityEngine;

namespace _project.Scripts.PlanB.Hengel
{
    public class MovingHook : NetworkBehaviour
    {
        public Reeling _reeling;
        public Vector3 moveDirection;
        public Quaternion rotateDirection;

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            moveDirection = Vector3.zero;
            rotateDirection = Quaternion.identity;
            ulong localClientId = NetworkManager.LocalClientId;

            if (localClientId == _reeling.verticalPlayerId)
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    moveDirection.x -= _reeling.currentSpeed * Time.deltaTime;
                    rotateDirection = Quaternion.Euler(0, 0, 90);
                }

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    moveDirection.x += _reeling.currentSpeed * Time.deltaTime;
                    rotateDirection = Quaternion.Euler(0, 0, -90);
                }

                // Scale animator speed based on current speed
                float maxSpeed = 10f;  // Adjust max speed accordingly
                float slowdownFactor = 0.0005f;
                float currentSpeed = maxSpeed * (1 / (1 + _reeling.distance * slowdownFactor));
                _animator.speed = Mathf.Clamp(_reeling.currentSpeed / _reeling.distance, 0.5f, 1f);
                //Debug.Log(_animator.speed);
            }

            if (moveDirection != Vector3.zero)
            {
                _reeling.MoveHookServerRpc(moveDirection);
            }

            if (rotateDirection != Quaternion.identity)
            {
                RotatePlayerServerRpc(rotateDirection);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void RotatePlayerServerRpc(Quaternion rotate, ServerRpcParams rpcParams = default)
        {
            transform.rotation = rotate;
            RotatePlayerClientRpc(transform.rotation);
        }

        [ClientRpc]
        private void RotatePlayerClientRpc(Quaternion newRot)
        {
            if (!IsOwner) transform.rotation = newRot;
        }

        private void OnTriggerStay(Collider other)
        {
            if (Input.GetKeyDown(KeyCode.C) && other.CompareTag("Fish"))
            {
                gameObject.GetComponent<Catch>().Invoke(other.gameObject.GetComponent<Fish>());
            }
        }
    }
}