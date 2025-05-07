using System;
using FishSystem;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Snap : ICommand
{
    [SerializeField] private Material _myMaterial;
    public bool _isBuildingBlock = true;
    public int placed;
    public int isPickedUp;

    public GameObject UIplace;

    public SnapPosition _snapPosition;
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = false;
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
                    placed ++;
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
        if(GetComponent<Snap>().isPickedUp > 0){
            
            GameObject referenceObject = col.gameObject.transform.parent.gameObject;
            
            // Get the forward direction in the horizontal plane
            Vector3 refForward = referenceObject.transform.forward;
            refForward.y = 0;
            refForward.Normalize();

            // Get 90° perpendicular direction (right turn)
            Vector3 perpDirection = new Vector3(-refForward.z, 0, refForward.x);

            if (perpDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(perpDirection, Vector3.up);
                transform.rotation = targetRotation;
                transform.rotation = new quaternion(transform.rotation.x,transform.rotation.y + 90,transform.rotation.z + 90,0);
                if (col.tag != "Ground" ){
                    print("came here 1");
                    transform.position = new Vector3(col.transform.position.x, transform.position.y, col.transform.position.z);
                }
                else if (col.tag == "Ground"){
                    print("came here 2");
                    col.tag = "Untagged";
                    col.enabled = false;
                }
                GetComponent<Rigidbody>().isKinematic = true;
            }            
        }
    }

}
