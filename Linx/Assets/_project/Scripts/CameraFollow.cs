using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    public float speed;

    public float distanceForMovement;

    public Vector3 newPosition;
    public float notmoving;

    public Vector3 movetoPlayerPos;

    public bool isnotmoving;


    private void Start()
    {
        newPosition.z = -10f;
        movetoPlayerPos.z = -10f;
    }

    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;

        if (Mathf.Abs(direction.x) > distanceForMovement || Mathf.Abs(direction.y) > distanceForMovement)
        {
            newPosition = transform.position + new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
        
            
            newPosition.z = transform.position.z;

            isnotmoving = false;

        }

        if ( transform.position != newPosition && isnotmoving == false && notmoving >= 0)
        {
            
            transform.position = newPosition;
            
            if (notmoving <= 5f)
            {
                notmoving++;
            }

        }
        else if(transform.position == newPosition && notmoving >= 0)
        {
            notmoving -= Time.deltaTime;

            if (notmoving <= 0)
            { 
                notmoving++;
               
                movetoPlayerPos = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z), speed * Time.deltaTime);

                transform.position = movetoPlayerPos;
            }
        }
    }
}
