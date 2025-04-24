using System;
using FishSystem;
using Unity.VisualScripting;
using UnityEngine;

public class Snap : ICommand
{
    public bool _isBuildingBlock = true;
    public int placed;
    void OnTriggerStay(Collider other)
    {
        if (_isBuildingBlock && Input.GetKeyDown(KeyCode.E)){
            Invoke(other);
            _isBuildingBlock = false;
            placed ++;
        }
    }
    void OnTriggerExit(Collider other)
    {
        placed --;
        _isBuildingBlock = true;
    }
    public override void Invoke(Fish fish)
    {
        throw new System.NotImplementedException();
    }
    public override void Invoke(Collider col)
    {

        GameObject referenceObject = col.gameObject;

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
