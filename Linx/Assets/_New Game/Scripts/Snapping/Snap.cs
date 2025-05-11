using System;
using FishSystem;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Snap : ICommand
{
    [SerializeField] private Material _myMaterial;
    public bool _isBuildingBlock = true;
    public int placed;
    public int isPickedUp;

    public GameObject UIplace;

    public SnapPosition _snapPosition;
    public Rigidbody rb;
    
    public NetworkVariable<bool> iskinematicnet = new NetworkVariable<bool>(true,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = false;

        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "BuildPosition")
        {
            _snapPosition = other.GetComponent<SnapPosition>();
            
            if (_snapPosition.hasObjectsInHere == false)
            {
                if (_isBuildingBlock && Input.GetKeyDown(KeyCode.F))
                {
                    
                    Invoke(other);
                    _isBuildingBlock = false;
                    _snapPosition.setTrue(true);
                    placed++;
                }

                if (_isBuildingBlock)
                {
                    _myMaterial.color = Color.green;
                    UIplace.SetActive(true);
                }
                else
                {
                    _myMaterial.color = Color.yellow;
                    UIplace.SetActive(false);
                }
            }
        }
        
    }
    void OnTriggerExit(Collider other)
    {
        //placed --;
        _isBuildingBlock = true;
        _myMaterial.color = Color.yellow;
        UIplace.SetActive(false);
    }
    public override void Invoke(Fish fish)
    {
        throw new System.NotImplementedException();
    }
    public override void Invoke(Collider col)
    {
        if (isPickedUp > 0)
        {
            Vector3 colPosition = col.transform.position;
            Quaternion colRotation = col.transform.rotation;
            string coltag = col.tag;
            
            
            RequestSnappingServerRpc(colPosition, coltag, colRotation );
        }
        
        
    }
    
    [ServerRpc(RequireOwnership = false)]
    void RequestSnappingServerRpc(Vector3 colposition, string coltag, Quaternion colRotation)
    {
        GameObject referenceObject = transform.parent != null ? transform.parent.gameObject : gameObject;
        
        Vector3 refForward = referenceObject.transform.forward;
        refForward.y = 0;
        refForward.Normalize();
        
        Vector3 perpDirection = new Vector3(-refForward.z, 0, refForward.x);
        Quaternion targetRotation = Quaternion.LookRotation(colRotation * perpDirection, Vector3.up);

        Vector3 newposition = (coltag != "Ground")
            ? new Vector3(colposition.z, transform.position.y, colposition.z)
            : transform.position;
        
        iskinematicnet.Value = true;

        if (coltag == "Ground")
        {

            Collider groundcollider =
                Physics.OverlapSphere(_snapPosition.transform.position, 0.1f).FirstOrDefault(c => c.CompareTag("Ground"));
            

                if (groundcollider != null)
                {
                    groundcollider.enabled = false;
                    groundcollider.gameObject.tag = "BuildPosition";

                }
            
        }
        ClientRequestSnappingClientRpc(newposition, targetRotation);
                      
        
    }

    [ClientRpc]
    void ClientRequestSnappingClientRpc(Vector3 newposition, Quaternion newRotation)
    {
        transform.position = newposition;
        transform.rotation = newRotation;
    }

    private void OnEnable()
    {
        print("check");
        iskinematicnet.OnValueChanged += OnKinematicChanged;
    }

    private void OnDisable()
    {
        print("heck");
        iskinematicnet.OnValueChanged -= OnKinematicChanged;
    }

    private void OnKinematicChanged(bool OldValue, bool newValue)
    {
        print("fuck");
        rb.isKinematic = newValue;
    }
}
