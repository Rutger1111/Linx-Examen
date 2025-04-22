using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class PickUp : NetworkBehaviour
{
    [SerializeField] 
    private float _range = 2f;

    [SerializeField] 
    public  GameObject _pickUpPosition;

    [SerializeField] 
    private string _targetTag = "moveAbleObject";

    private NetworkObject _heldObject;

    private List<GameObject> _pickUpAbleObjects = new List<GameObject>();
    private ConfigurableJoint _joint;

    void Update()
    {
        if (!IsOwner) return;

        FindNearbyObjects();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_heldObject == null && _pickUpAbleObjects.Count > 0)
            {
                print("heck");
                ulong targetId = _pickUpAbleObjects[0].GetComponent<NetworkObject>().NetworkObjectId;
                RequestPickUpServerRpc(targetId);
            }
            else if (_heldObject != null)
            {
                print("check");
                ulong targetId = _heldObject.NetworkObjectId;
                RequestDropServerRpc(targetId);
            }
        }

        if (_heldObject != null)
        {
            print("heeftobject");
            if (_joint == null){
                _heldObject.transform.position = _pickUpPosition.transform.position;
                _joint = _pickUpPosition.AddComponent<ConfigurableJoint>();
                _joint.connectedBody = _heldObject.GetComponent<Rigidbody>();
                _joint.xMotion = ConfigurableJointMotion.Limited;
                _joint.yMotion = ConfigurableJointMotion.Limited;
                _joint.zMotion = ConfigurableJointMotion.Limited;
            }
            
        }
        else{
            if (_joint != null){
                Destroy(_joint);
                _joint = null;
            }
        }
    }

    void FindNearbyObjects()
    {
        _pickUpAbleObjects.Clear();
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag(_targetTag);

        foreach (GameObject obj in allObjects)
        {
            float distance = Vector3.Distance(_pickUpPosition.transform.position, obj.transform.position);
            if (distance <= _range)
            {
                _pickUpAbleObjects.Add(obj);
            }
        }
    }

    [ServerRpc]
    void RequestPickUpServerRpc(ulong objectId, ServerRpcParams rpcParams = default)
    {
        if (!NetworkManager.SpawnManager.SpawnedObjects.ContainsKey(objectId))
            return;

        NetworkObject objNet = NetworkManager.SpawnManager.SpawnedObjects[objectId];

        // Optional: Limit pickup to unheld items
        if (objNet.IsOwnedByServer || !objNet.IsOwnedByServer && !objNet.IsOwnershipTransferable)
        {
            objNet.ChangeOwnership(rpcParams.Receive.SenderClientId);

            Gravity gravity = objNet.GetComponent<Gravity>();
            if (gravity != null) gravity.hasGravity = false;

            ConfirmPickUpClientRpc(objectId);
        }
    }

    [ClientRpc]
    void ConfirmPickUpClientRpc(ulong objectId)
    {
        if (IsOwner)
        {
            _heldObject = NetworkManager.SpawnManager.SpawnedObjects[objectId];
        }
    }

    [ServerRpc]
    void RequestDropServerRpc(ulong objectId, ServerRpcParams rpcParams = default)
    {
        if (!NetworkManager.SpawnManager.SpawnedObjects.ContainsKey(objectId))
            return;

        NetworkObject objNet = NetworkManager.SpawnManager.SpawnedObjects[objectId];

        Gravity gravity = objNet.GetComponent<Gravity>();
        if (gravity != null) gravity.hasGravity = true;

        objNet.RemoveOwnership();

        ConfirmDropClientRpc(objectId);
    }

    [ClientRpc]
    void ConfirmDropClientRpc(ulong objectId)
    {
        if (IsOwner && _heldObject != null && _heldObject.NetworkObjectId == objectId)
        {
            _heldObject = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (_pickUpPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_pickUpPosition.transform.position, _range);
        }
    }
}
