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

    private NetworkObject netobj;
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = false;

        netobj = GetComponent<NetworkObject>();
    }

    private void Update()
    {
        if (_isBuildingBlock == false)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "BuildPosition")
        {
            _snapPosition = other.GetComponent<SnapPosition>();
            
            Debug.Log(_snapPosition.gameObject);
            
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
            string coltag = col.tag;
            
            
            RequestSnapping(colPosition, coltag );
        }
        
        
    }
    
    [ServerRpc(RequireOwnership = false)]
    void RequestSnapping(Vector3 colposition, string coltag)
    {
        GameObject referenceObject = transform.parent != null ? transform.parent.gameObject : gameObject;
        
        Vector3 refForward = referenceObject.transform.forward;
        refForward.y = 0;
        refForward.Normalize();
        
        Vector3 perpDirection = new Vector3(-refForward.z, 0, refForward.x);
        Quaternion targetRotation = Quaternion.LookRotation(perpDirection, Vector3.up);

        Vector3 newposition = (coltag != "Ground")
            ? new Vector3(colposition.z, transform.position.y, colposition.z)
            : transform.position;

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
        ClientRequestSnapping(newposition, targetRotation);
                      
        
    }

    [ClientRpc]
    void ClientRequestSnapping(Vector3 newposition, Quaternion newRotation)
    {
        transform.position = newposition;
        transform.rotation = newRotation;
        GetComponent<Rigidbody>().isKinematic = true;
    }

}
