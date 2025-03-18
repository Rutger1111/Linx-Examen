using System.Collections.Generic;
using UnityEngine;

public class ClintWalking : MonoBehaviour
{
    public List<GameObject> waypoints;

    public int speed;

    public Vector3 target;

    public int index = 0;
    void Start()
    {
        
    }

    
    void Update()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (Vector3.Distance(transform.position, waypoints[i].transform.position) > 0.00001f)
            {
                target = waypoints[index].transform.position;
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }
            else
            {
                if (index < waypoints.Count - 1)
                {
                    index++;
                }
                else
                {
                    
                }
                
            }
            
        }

        
    }
}
