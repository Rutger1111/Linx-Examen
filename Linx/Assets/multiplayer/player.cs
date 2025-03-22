using System;
using UnityEngine;
using Photon.Pun;

public class player : MonoBehaviour
{
    PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (photonView.IsMine)
        {
            createController();
        }
    }

    void createController()
    {
        
    }
}
