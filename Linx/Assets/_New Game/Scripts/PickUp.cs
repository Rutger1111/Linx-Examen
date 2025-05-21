using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace _New_Game.Scripts
{
    public class PickUp : NetworkBehaviour
    {
        [SerializeField] private float range = 2f;
        [SerializeField] public GameObject pickUpPosition;
        [SerializeField] private string targetTag = "moveAbleObject";
    
        private NetworkObject _heldObject;
        public List<GameObject> pickUpAbleObjects = new List<GameObject>();
        private ConfigurableJoint _joint;

        public GameObject[] allObjects;

        void Update()
        {

            FindNearbyObjects();

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_heldObject == null && pickUpAbleObjects.Count > 0)
                {
                    TryPickUp();
                    _heldObject.GetComponent<FixedJoint>().connectedBody.gameObject.GetComponent<Snap>().isPickedUp++;
                }
                else if (_heldObject != null)
                {
                    ulong targetId = _heldObject.NetworkObjectId;
                    Drop(targetId);
                }
            }

            UpdateJointLogic();
        }

        public void Drop(ulong targetId)
        {
            RequestDropServerRpc(targetId);
        }

        private void TryPickUp()
        {
            foreach (var obj in pickUpAbleObjects)
            {
                var pickupable = obj.GetComponent<PickUpItem>();
                if (pickupable == null || pickupable.IsHeld.Value) continue;
                ulong targetId = obj.GetComponent<NetworkObject>().NetworkObjectId;
                RequestPickUpServerRpc(targetId, NetworkObjectId);
                break;
            }
        }

        private void UpdateJointLogic()
        {
            if (_heldObject != null)
            {
                if (_joint != null) return;
                _heldObject.transform.position = pickUpPosition.transform.position;
                _joint = pickUpPosition.AddComponent<ConfigurableJoint>();
                _joint.connectedBody = _heldObject.GetComponent<Rigidbody>();
                _joint.xMotion = ConfigurableJointMotion.Limited;
                _joint.yMotion = ConfigurableJointMotion.Limited;
                _joint.zMotion = ConfigurableJointMotion.Limited;
                _joint.breakForce = 1;

            }
            else
            {
                if (_joint == null) return;
                Destroy(_joint);
                _joint = null;
            }
        }

        void FindNearbyObjects()
        {
            pickUpAbleObjects.Clear();
            allObjects = GameObject.FindGameObjectsWithTag(targetTag);

            GameObject closestObject = null;
        
            float closestDistance = Mathf.Infinity;

            foreach (GameObject obj in allObjects)
            {
                float currentDistance = Vector3.Distance(pickUpPosition.transform.position, obj.transform.position);

                if (currentDistance <= range && currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestObject = obj;
                }
            }

            if (closestObject != null)
            {
                pickUpAbleObjects.Add(closestObject);
            }
        }

        [ServerRpc]
        private void RequestPickUpServerRpc(ulong targetId, ulong playerId, ServerRpcParams rpcParams = default)
        {
            if (!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(targetId, out NetworkObject targetObject)) return;
            if (!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(playerId, out _)) return;

            var pickupable = targetObject.GetComponent<PickUpItem>();
            if (pickupable == null || pickupable.IsHeld.Value) return;

            pickupable.IsHeld.Value = true;

            if (targetObject.IsOwnedByServer || targetObject.IsOwnershipTransferable)
            {
                targetObject.ChangeOwnership(rpcParams.Receive.SenderClientId);

                var gravity = targetObject.GetComponent<Gravity>();
                if (gravity != null) gravity.hasGravity = false;

                ConfirmPickUpClientRpc(targetId);
            }
        }

        [ClientRpc]
        private void ConfirmPickUpClientRpc(ulong objectId)
        {
            if (IsOwner && NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId, out NetworkObject obj))
            {
                _heldObject = obj;
            }
        }

        [ServerRpc]
        private void RequestDropServerRpc(ulong objectId)
        {
            if (!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId, out NetworkObject objNet)) return;

            var pickupable = objNet.GetComponent<PickUpItem>();
            if (pickupable != null)
            {
                pickupable.IsHeld.Value = false;
            }

            var gravity = objNet.GetComponent<Gravity>();
            if (gravity != null) gravity.hasGravity = true;

            objNet.RemoveOwnership();

            ConfirmDropClientRpc(objectId);
        }

        [ClientRpc]
        private void ConfirmDropClientRpc(ulong objectId)
        {
            if (IsOwner && _heldObject != null && _heldObject.NetworkObjectId == objectId)
            {
                _heldObject.GetComponent<FixedJoint>().connectedBody.gameObject.GetComponent<Snap>().isPickedUp--;
                _heldObject = null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (pickUpPosition != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(pickUpPosition.transform.position, range);
            }
        }
    }
}