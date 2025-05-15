using System;
using FishSystem;
using ParrelSync.NonCore;
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
    public SnapPosition _snapPosition;
    public GameObject _hookObject1;
    public GameObject _hookObject2;
    
    private bool isInValidTrigger = false;
    private bool isplaced = false;
    public Vector3 colposition;
    public Quaternion colRotation;
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        UIplace.SetActive(false);
    }

    private void Update()
    {
        if (isInValidTrigger && _isBuildingBlock && Input.GetKeyDown(KeyCode.F))
        {
            Invoke();
            _isBuildingBlock = false;
            _snapPosition.setTrue(true);
            placed++;
        }

        if (isplaced == true)
        {
            transform.position = colposition;
            quaternion inverse =  Quaternion.Inverse(colRotation);

            transform.rotation = inverse;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (placed >= 1) return;

        if (other.CompareTag("BuildPosition"))
        {
            SnapPosition pos = other.GetComponent<SnapPosition>();

            if (pos != null && !pos.hasObjectsInHere)
            {
                _snapPosition = pos;
                UIplace.SetActive(true);
                isInValidTrigger = true;

                // Save the exact position and rotation of the collider
                colposition = other.transform.position;
                colRotation = other.transform.rotation;
            }
            else
            {
                UIplace.SetActive(false);
                isInValidTrigger = false;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        _isBuildingBlock = true;
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