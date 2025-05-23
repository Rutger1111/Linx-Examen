<<<<<<< HEAD
using System.Collections.Generic;
using FishSystem;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace _New_Game.Scripts.Snapping
{
    public class Snap : MonoBehaviour
    {
        private bool _isBuildingBlock = true;
        private int _placed;
        public int isPickedUp;
        [SerializeField] private bool isWallRoof;
        private Vector3 _pos;
        private Quaternion _rot;
        private Vector3 _hookPos1;
        private Quaternion _hookRot1;
        private Vector3 _hookPos2;
        private Quaternion _hookRot2;
        public List<SnapPosition> snapPosition = new List<SnapPosition>();
        public GameObject hookObject1;
        public GameObject hookObject2;
    
        private bool _isInValidTrigger;
        private bool _isPlaced;
        private Vector3 _colPosition;
        private Quaternion _colRotation;
        public int snapId;

        public GameObject invisibleWall;
        public GameObject decoratedWall;
        public GameObject thisWall;

        public bool blockPlaced;
        [SerializeField] private GameObject _UIPress;
        

        private List<ulong> _playersConfirmed = new List<ulong>();
        private float _firstPressTime = 0f;
        private float _timeWindow = 10f;
        private bool _placementConfirmed;
        void Start()
        {
            _rot = transform.rotation;
            _pos = transform.position;
            _hookPos1 = hookObject1.transform.position;
            _hookRot1 = hookObject1.transform.rotation;
            _hookPos2 = hookObject2.transform.position;
            _hookRot2 = hookObject2.transform.rotation;
        
            GetComponent<Rigidbody>().isKinematic = false;
        }

        private void Update()
        {
            _firstPressTime += Time.deltaTime;
            
            if (snapPosition.Count > 0)
            {
                foreach (var snapPos in snapPosition)
                {
                    if (!snapPos.hasObjectsInHere && snapPos.snapId == snapId)
                    {
                        _UIPress.SetActive(true);
                        _colPosition = snapPos.transform.position;
                        _colRotation = snapPos.transform.rotation;
                        _isInValidTrigger = true;
                    }
                    else if (_isBuildingBlock == false)
                    {
                        _isInValidTrigger = false;
                    }
                }
            }


            if (_isInValidTrigger && _isBuildingBlock && !blockPlaced)
            {

                if (Input.GetKeyDown(KeyCode.F))
                {
                    ConfirmPlacementServerRpc();
                }
            }


            if (_isPlaced)
            {
                transform.position = _colPosition;
                transform.rotation = _colRotation;
            }
        
            if (_firstPressTime > _timeWindow)
            {
                _playersConfirmed.Clear();
                _firstPressTime = 0;
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("BuildPosition"))
            {
                SnapPosition snap = other.GetComponent<SnapPosition>();
            
                if (snap != null && !snapPosition.Contains(snap))
                {
                    snapPosition.Add(snap);
                }
            }
        }
        void OnTriggerExit(Collider other)
        {
            snapPosition.Clear();
        }
        public void Invoke()
        {
            if (isPickedUp > 0)
            {
                _isPlaced = true;
            
                hookObject1.SetActive(false);
                hookObject2.SetActive(false);
            }
=======
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

public class Snap : NetworkBehaviour
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


    public List<ulong> playersConfirmed = new List<ulong>();
    public float firstPressTime = 0f;
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
        
//        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void Update()
    {
        firstPressTime += Time.deltaTime;
        
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
        
        if (firstPressTime > timeWindow)
        {
            playersConfirmed.Clear();
            firstPressTime = 0f;
>>>>>>> backup
        }

<<<<<<< HEAD
        [ServerRpc(RequireOwnership = false)]
        public void ConfirmPlacementServerRpc(ServerRpcParams rpcParams = default)
        {
            ulong clienId = rpcParams.Receive.SenderClientId;

            if (!_playersConfirmed.Contains(clienId))
            {
                _playersConfirmed.Add(clienId);
                if (_playersConfirmed.Count == 2 && Time.time - _firstPressTime <= _timeWindow)
                {
                    _placementConfirmed = true;
                    PlaceBlockClientRpc();
                }
                else if (_firstPressTime > _timeWindow)
                {
                    _playersConfirmed.Clear();
                    _firstPressTime = 0f;
                }
            }
        }
    
        [ClientRpc]
        private void PlaceBlockClientRpc()
        {
            blockPlaced = true;
            _isPlaced = true;
            _isBuildingBlock = false;
            _UIPress.SetActive(false);
            
            invisibleWall.SetActive(false);
            decoratedWall.SetActive(true);
            thisWall.SetActive(false);
        
            hookObject1.SetActive(false);
            hookObject2.SetActive(false);
        }
    
    }
=======
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

    [ServerRpc(RequireOwnership = false)]
    public void confirmPlacementServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clienId = rpcParams.Receive.SenderClientId;

        if (!playersConfirmed.Contains(clienId))
        {
            playersConfirmed.Add(clienId);
            
            if (playersConfirmed.Count == 2 && firstPressTime <= timeWindow)
            {
                placementConfirmed = true;
                PlaceBlockClientRpc();
            }
            else if (firstPressTime > timeWindow)
            {
                playersConfirmed.Clear();
                firstPressTime = 0f;
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
    
>>>>>>> backup
}