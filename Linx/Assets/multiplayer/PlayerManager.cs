using System;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (pv.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
    }
}
