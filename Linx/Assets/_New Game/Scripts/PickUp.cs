using System.Collections.Generic;
using _New_Game.Scripts;
using _New_Game.Scripts.Snapping;
using Unity.Netcode;
using UnityEngine;

public class PickUp : NetworkBehaviour
{
    [SerializeField] private float _range = 2f;
    [SerializeField] public GameObject _pickUpPosition;
    [SerializeField] private string _targetTag = "moveAbleObject";
    
    private NetworkObject _heldObject;
    public List<GameObject> _pickUpAbleObjects = new List<GameObject>();
    private ConfigurableJoint _joint;
    
<<<<<<< HEAD
=======

    public GameObject[] allObjects;
>>>>>>> backup

    public GameObject[] allObjects;

    private void Update()
    {

        FindNearbyObjects();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_heldObject == null && _pickUpAbleObjects.Count > 0)
            {
                TryPickUp();
                _heldObject.GetComponent<PickUpItem>().Wall.gameObject.GetComponent<Snap>().isPickedUp++;
            }
            else if (_heldObject != null)
            {
                ulong targetId = _heldObject.NetworkObjectId;
                drop(targetId);
            }
        }

        UpdateJointLogic();
    }

    public void drop(ulong targetId)
    {
        RequestDropServerRpc(targetId);
    }
<<<<<<< HEAD

    private void TryPickUp()
=======
    void TryPickUp()
>>>>>>> backup
    {
        foreach (var obj in _pickUpAbleObjects)
        {
            var pickupable = obj.GetComponent<PickUpItem>();
            if (pickupable != null && !pickupable.IsHeld.Value)
            {
                ulong targetId = obj.GetComponent<NetworkObject>().NetworkObjectId;
                RequestPickUpServerRpc(targetId);
                break;
            }
        }
    }

    private void UpdateJointLogic()
    {
        if (_heldObject != null)
        {
            if (_joint == null){
                _heldObject.transform.position = _pickUpPosition.transform.position;
                _joint = _pickUpPosition.AddComponent<ConfigurableJoint>();
                _joint.connectedBody = _heldObject.GetComponent<Rigidbody>();
                _joint.xMotion = ConfigurableJointMotion.Limited;
                _joint.yMotion = ConfigurableJointMotion.Limited;
                _joint.zMotion = ConfigurableJointMotion.Limited;
                _joint.breakForce = 1;
            }
            
        }
        else{
            if (_joint != null){
                Destroy(_joint);
                _joint = null;
            }
        }
    }

    private void FindNearbyObjects()
    {
        _pickUpAbleObjects.Clear();
        allObjects = GameObject.FindGameObjectsWithTag(_targetTag);

        GameObject closestObject = null;
        
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in allObjects)
        {
            float currentDistance = Vector3.Distance(_pickUpPosition.transform.position, obj.transform.position);

            if (currentDistance <= _range && currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestObject = obj;
            }
        }

        if (closestObject != null)
        {
            _pickUpAbleObjects.Add(closestObject);
        }
    }

    [ServerRpc]
<<<<<<< HEAD
    private void RequestPickUpServerRpc(ulong targetId, ulong playerId, ServerRpcParams rpcParams = default)
=======
    void RequestPickUpServerRpc(ulong targetId, ServerRpcParams rpcParams = default)
>>>>>>> backup
    {
        if (!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(targetId, out NetworkObject targetObject)) return;

        var pickupable = targetObject.GetComponent<PickUpItem>();
        if (pickupable == null || pickupable.IsHeld.Value) return;

        pickupable.IsHeld.Value = true;

        if (targetObject.IsOwnedByServer || targetObject.IsOwnershipTransferable)
        {
            targetObject.ChangeOwnership(rpcParams.Receive.SenderClientId);

            Gravity gravity = targetObject.GetComponent<Gravity>();
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
    private void RequestDropServerRpc(ulong objectId, ServerRpcParams rpcParams = default)
    {
        if (!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId, out NetworkObject objNet)) return;

        var pickupable = objNet.GetComponent<PickUpItem>();
        if (pickupable != null)
        {
            pickupable.IsHeld.Value = false;
        }

        Gravity gravity = objNet.GetComponent<Gravity>();
        if (gravity != null) gravity.hasGravity = true;

        objNet.RemoveOwnership();

        ConfirmDropClientRpc(objectId);
    }

    [ClientRpc]
    private void ConfirmDropClientRpc(ulong objectId)
    {
        if (IsOwner && _heldObject != null && _heldObject.NetworkObjectId == objectId)
        {
            _heldObject.GetComponent<PickUpItem>().Wall.gameObject.GetComponent<Snap>().isPickedUp--;
            _heldObject = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_pickUpPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_pickUpPosition.transform.position, _range);
        }
    }
}