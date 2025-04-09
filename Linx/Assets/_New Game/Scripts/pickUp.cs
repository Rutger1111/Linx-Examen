using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class pickUp : NetworkBehaviour
{
    public float range = 2f;

    public GameObject pickUpPosition;
    public GameObject dropPosition;

    public string targetTag = "moveAbleObject";

    private NetworkObject heldObject;

    private List<GameObject> pickUpAbleObjects = new List<GameObject>();

    void Update()
    {
        if (!IsOwner) return;

        FindNearbyObjects();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null && pickUpAbleObjects.Count > 0)
            {
                print("heck");
                ulong targetId = pickUpAbleObjects[0].GetComponent<NetworkObject>().NetworkObjectId;
                RequestPickUpServerRpc(targetId);
            }
            else if (heldObject != null)
            {
                print("check");
                ulong targetId = heldObject.NetworkObjectId;
                RequestDropServerRpc(targetId);
            }
        }

        if (heldObject != null)
        {
            heldObject.transform.position = pickUpPosition.transform.position;
        }
    }

    void FindNearbyObjects()
    {
        pickUpAbleObjects.Clear();
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in allObjects)
        {
            float distance = Vector3.Distance(pickUpPosition.transform.position, obj.transform.position);
            if (distance <= range)
            {
                pickUpAbleObjects.Add(obj);
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
            heldObject = NetworkManager.SpawnManager.SpawnedObjects[objectId];
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
        if (IsOwner && heldObject != null && heldObject.NetworkObjectId == objectId)
        {
            heldObject = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (pickUpPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pickUpPosition.transform.position, range);
        }
    }
}
