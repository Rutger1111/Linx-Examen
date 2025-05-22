using System;
using System.Collections.Generic;
using FishSystem;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Unity.Netcode;

public class Snap : ICommand
{
    public bool _isBuildingBlock = true;
    public int placed;
    public int isPickedUp;
    [SerializeField] private bool isWallRoof = false;
    private Vector3 _pos;
    private Quaternion _rot;
    private Vector3 _hookPos1;
    private Quaternion _hookRot1;
    private Vector3 _hookPos2;
    private Quaternion _hookRot2;
    public List<SnapPosition> _snapPosition = new List<SnapPosition>();
    public GameObject _hookObject1;
    public GameObject _hookObject2;
    
    private bool isInValidTrigger = false;
    private bool isplaced = false;
    public Vector3 colposition;
    public Quaternion colRotation;
    public int snapId;

    public GameObject invisableWall;
    public GameObject decoratedWall;
    public GameObject thisWall;

    public bool blockPlaced;


    private List<ulong> playersConfirmed = new List<ulong>();
    private float firstPressTime = -1f;
    private float timeWindow = 10f;
    private bool placementConfirmed = false;
    void Start()
    {
        _rot = transform.rotation;
        _pos = transform.position;
        _hookPos1 = _hookObject1.transform.position;
        _hookRot1 = _hookObject1.transform.rotation;
        _hookPos2 = _hookObject2.transform.position;
        _hookRot2 = _hookObject2.transform.rotation;
        
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void Update()
    {
        if (_snapPosition.Count > 0)
        {
            foreach (var snapPos in _snapPosition)
            {
                if (!snapPos.hasObjectsInHere && snapPos.snapId == snapId)
                {
                    colposition = snapPos.transform.position;
                    colRotation = snapPos.transform.rotation;
                    isInValidTrigger = true;
                }
                else if (_isBuildingBlock == false)
                {
                    isInValidTrigger = false;
                }
            }
        }


        if (isInValidTrigger && _isBuildingBlock && !blockPlaced)
        {

            if (Input.GetKeyDown(KeyCode.F))
            {
                confirmPlacementServerRpc();
            }
        }


        if (isplaced)
        {
            transform.position = colposition;
            transform.rotation = colRotation;
        }
        
        if (firstPressTime > 0 && Time.time - firstPressTime > timeWindow)
        {
            playersConfirmed.Clear();
            firstPressTime = -1f;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BuildPosition"))
        {
            SnapPosition snap = other.GetComponent<SnapPosition>();
            
            if (snap != null && !_snapPosition.Contains(snap))
            {
                _snapPosition.Add(snap);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        _snapPosition.Clear();
    }
    public override void Invoke(Fish fish)
    {
        throw new System.NotImplementedException();
    }
    public void Invoke()
    {
        if (isPickedUp > 0)
        {
            isplaced = true;
            
            _hookObject1.SetActive(false);
            _hookObject2.SetActive(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void confirmPlacementServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clienId = rpcParams.Receive.SenderClientId;

        if (!playersConfirmed.Contains(clienId))
        {
            playersConfirmed.Add(clienId);
            if (playersConfirmed.Count == 1)
            {
                firstPressTime = Time.time;
            }
            else if (playersConfirmed.Count == 2 && Time.time - firstPressTime <= timeWindow)
            {
                placementConfirmed = true;
                PlaceBlockClientRpc();
            }
            else if (Time.time - firstPressTime > timeWindow)
            {
                playersConfirmed.Clear();
                firstPressTime = -1f;
            }
        }
    }
    
    [ClientRpc]
    private void PlaceBlockClientRpc()
    {
        blockPlaced = true;
        isplaced = true;
        _isBuildingBlock = false;

        invisableWall.SetActive(false);
        decoratedWall.SetActive(true);
        thisWall.SetActive(false);
        
        _hookObject1.SetActive(false);
        _hookObject2.SetActive(false);
    }
    
}