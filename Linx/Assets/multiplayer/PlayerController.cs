using System;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    
    private Rigidbody rb;

    private Vector3 moveAmount;

    private PhotonView vw;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        vw = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (!vw.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }
    }
    private void Update()
    {
        if (!vw.IsMine)
            return;

        
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        
       
        moveAmount = movement * speed;
    }

    private void FixedUpdate()
    {
        if (!vw.IsMine)
            return;
        
        rb.linearVelocity = new Vector3(moveAmount.x, rb.linearVelocity.y, moveAmount.z);
    }
}
