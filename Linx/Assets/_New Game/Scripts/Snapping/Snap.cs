using System;
using System.Collections.Generic;
using FishSystem;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Snap : ICommand
{
    public bool _isBuildingBlock = true;
    public int placed;
    public int isPickedUp;
    [SerializeField] private bool isWallRoof = false;
    public GameObject UIplace;
    private Vector3 _pos;
    private quaternion _hook2Rot;
    
    public List<SnapPosition> _snapPosition = new List<SnapPosition>();
    public GameObject _hookObject1;
    public GameObject _hookObject2;
    
    private bool isInValidTrigger = false;
    private bool isplaced = false;
    public Vector3 colposition;
    public Quaternion colRotation;
    public int snapId;
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        UIplace.SetActive(false);
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
                    print("check");
                }
                else if (_isBuildingBlock == false)
                {
                    isInValidTrigger = false;
                    UIplace.SetActive(false);
                }
            }
        }

        if (isInValidTrigger && _isBuildingBlock == true)
        {
            UIplace.SetActive(true);
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                Invoke();
                _isBuildingBlock = false;
                placed++;
            
            }
        }
        

        if (isplaced)
        {
            transform.position = colposition;
            transform.rotation = colRotation;
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
        UIplace.SetActive(false);
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

            // Use the already passed position/rotation directly
            _hookObject1.SetActive(false);
            _hookObject2.SetActive(false);

            Debug.Log("Snap placement invoked");
        }
    }

}