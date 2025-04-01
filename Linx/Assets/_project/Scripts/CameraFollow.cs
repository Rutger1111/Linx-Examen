using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float distanceForMovement;
    public float notmovingTime = 3f; 
    public float notmoving;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
        notmoving = notmovingTime;
    }

    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;

        notmoving -= Time.deltaTime;
        
        if (Mathf.Abs(direction.x) > distanceForMovement || Mathf.Abs(direction.y) > distanceForMovement)
        {
            notmoving = notmovingTime;
            
            
            transform.position = Vector3.MoveTowards(transform.position, 
                new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z), speed * Time.deltaTime);
            
            
        }
        else if (notmoving <= 0)
        {
            transform.position = Vector3.Lerp(transform.position, 
                    new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z), speed * Time.deltaTime);
        }
    }
}
