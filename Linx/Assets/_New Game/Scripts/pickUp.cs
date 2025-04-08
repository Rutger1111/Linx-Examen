using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class pickUp : MonoBehaviour
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
            float distance = Vector3.Distance(transform.position, objItemInRange.transform.position);
            
            if (distance <= range)
            {
                pickUpAbleObjects.Add(objItemInRange);

                
            }
        }
        
        
        //float distance = Vector3.Distance(gameObject.transform.position, obj.transform.position);

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
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
