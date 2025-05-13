using System;
using FishSystem;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Snap : NetworkBehaviour
{
    [SerializeField] private Material _myMaterial;
    public bool _isBuildingBlock = true;
    public int placed;
    public int isPickedUp;

    public GameObject UIplace;

    public SnapPosition _snapPosition;
    public Rigidbody rb;

    public bool isplaced;
    
    private HashSet<ulong> playersConfirmed = new HashSet<ulong>();
    
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = false;

        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("BuildPosition")) return;

        _snapPosition = other.GetComponent<SnapPosition>();
        if (_snapPosition == null || _snapPosition.hasObjectsInHere || isplaced) return;

       
        
            ulong clientId= NetworkManager.Singleton.LocalClientId;
            

            if (_isBuildingBlock && Input.GetKeyDown(KeyCode.F) && isPickedUp > 0)
            {
                print("duck");
                    ConfirmPressedServerRpc(clientId);
                
            }

            _myMaterial.color = _isBuildingBlock ? Color.green : Color.yellow;
            UIplace.SetActive(_isBuildingBlock);
        

    }
    void OnTriggerExit(Collider other)
    {
        placed --;
        _isBuildingBlock = true;
        _myMaterial.color = Color.yellow;
        UIplace.SetActive(false);
    }
    
    [ServerRpc(RequireOwnership = false)]
    void ConfirmPressedServerRpc(ulong clientId)
    {
        print("fuck");
        
        if (!playersConfirmed.Contains(clientId))
            playersConfirmed.Add(clientId);

        if (playersConfirmed.Count >= 2)
        {
            Vector3 newPosition = _snapPosition.transform.position;
            Quaternion newRotation = _snapPosition.transform.rotation;

            print("check");
            
            rb.isKinematic = true;

            Collider groundCollider = Physics.OverlapSphere(_snapPosition.transform.position, 0.1f)
                .FirstOrDefault(c => c.CompareTag("Ground"));
            if (groundCollider != null)
            {
                groundCollider.enabled = false;
                groundCollider.gameObject.tag = "BuildPosition";
            }

            _snapPosition.setTrue(true);
            isplaced = true;
            placed++;

            FinalizeSnapClientRpc(newPosition, newRotation);
        }
    }

    [ClientRpc]
    void FinalizeSnapClientRpc(Vector3 newposition, Quaternion newrotation)
    {
        print("heck");
        
        transform.position = newposition;
        transform.rotation = newrotation;
        rb.isKinematic = true;
    }
}
