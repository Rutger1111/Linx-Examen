using System.Collections.Generic;
using FishSystem;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace _New_Game.Scripts.Snapping
{
    public class Snap : MonoBehaviour
    {
        public bool isBuildingBlock = true;
        public int placed;
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
        private bool _isplaced;
        public Vector3 colposition;
        public Quaternion colRotation;
        public int snapId;

        public GameObject invisableWall;
        public GameObject decoratedWall;
        public GameObject thisWall;

        public bool blockPlaced;


        private List<ulong> _playersConfirmed = new List<ulong>();
        private float _firstPressTime = -1f;
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
            if (snapPosition.Count > 0)
            {
                foreach (var snapPos in snapPosition)
                {
                    if (!snapPos.hasObjectsInHere && snapPos.snapId == snapId)
                    {
                        colposition = snapPos.transform.position;
                        colRotation = snapPos.transform.rotation;
                        _isInValidTrigger = true;
                    }
                    else if (isBuildingBlock == false)
                    {
                        _isInValidTrigger = false;
                    }
                }
            }


            if (_isInValidTrigger && isBuildingBlock && !blockPlaced)
            {

                if (Input.GetKeyDown(KeyCode.F))
                {
                    ConfirmPlacementServerRpc();
                }
            }


            if (_isplaced)
            {
                transform.position = colposition;
                transform.rotation = colRotation;
            }
        
            if (_firstPressTime > 0 && Time.time - _firstPressTime > _timeWindow)
            {
                _playersConfirmed.Clear();
                _firstPressTime = -1f;
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
                _isplaced = true;
            
                hookObject1.SetActive(false);
                hookObject2.SetActive(false);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ConfirmPlacementServerRpc(ServerRpcParams rpcParams = default)
        {
            ulong clienId = rpcParams.Receive.SenderClientId;

            if (!_playersConfirmed.Contains(clienId))
            {
                _playersConfirmed.Add(clienId);
                if (_playersConfirmed.Count == 1)
                {
                    _firstPressTime = Time.time;
                }
                else if (_playersConfirmed.Count == 2 && Time.time - _firstPressTime <= _timeWindow)
                {
                    _placementConfirmed = true;
                    PlaceBlockClientRpc();
                }
                else if (Time.time - _firstPressTime > _timeWindow)
                {
                    _playersConfirmed.Clear();
                    _firstPressTime = -1f;
                }
            }
        }
    
        [ClientRpc]
        private void PlaceBlockClientRpc()
        {
            blockPlaced = true;
            _isplaced = true;
            isBuildingBlock = false;

            invisableWall.SetActive(false);
            decoratedWall.SetActive(true);
            thisWall.SetActive(false);
        
            hookObject1.SetActive(false);
            hookObject2.SetActive(false);
        }
    
    }
}