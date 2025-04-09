using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class pickUp : NetworkBehaviour
{
    public float range;

    public GameObject pickUpPosition;
    public GameObject obj;
    
    public GameObject dropPosition;

    public string targetTag = "moveAbleObject";

    public bool hasObjectPickup = false;

    public List<GameObject> pickUpAbleObjects = new List<GameObject>();

    void Update()
    {
        pickUpAbleObjects.Clear(); 

        GameObject[] allObjects = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject objItemInRange in allObjects)
        {
            float distance = Vector3.Distance(pickUpPosition.transform.position, objItemInRange.transform.position);
            
            if (distance <= range)
            {
                pickUpAbleObjects.Add(objItemInRange);
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && hasObjectPickup == false)
        {
            hasObjectPickup = true;
            
            obj = pickUpAbleObjects[0];

            pickUpAbleObjects[0].GetComponent<Gravity>().hasGravity = false;

        }
        else if (Input.GetKeyDown(KeyCode.E) && hasObjectPickup == true)
        {
            hasObjectPickup = false;
            obj = null;
            
            pickUpAbleObjects[0].GetComponent<Gravity>().hasGravity = true;
            
        }

        if (hasObjectPickup)
        {
            obj.transform.position = pickUpPosition.transform.position;
        }
        
        snapBack();
    }

    public void snapBack()
    {
        if (hasObjectPickup == false)
        {
            
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pickUpPosition.transform.position, range);
    }
}
