using System.Collections.Generic;
using FishSystem;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace _New_Game.Scripts.Snapping
{
    public class Snap : NetworkBehaviour
    {
        public bool _isBuildingBlock = true;
        public int _placed;
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
        [SerializeField] private TMP_Text _textUI;
        
        

        public List<ulong> _playersConfirmed = new List<ulong>();
        public float _firstPressTime = 0f;
        private float _timeWindow = 10f;
        private bool _placementConfirmed;
        private int _confirmCounter;
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

            _confirmCounter = _playersConfirmed.Count;
            _textUI.text = "Press F to Place" + _confirmCounter;
            
            foreach (var snapPos in snapPosition)
            {
                if (!snapPos.hasObjectsInHere && snapPos.snapId == snapId)
                {
                        _UIPress.SetActive(true);
                        _colPosition = snapPos.transform.position;
                        _colRotation = snapPos.transform.rotation;
                        _isInValidTrigger = true;
                }
                else
                {
                        _UIPress.SetActive(false);
                }
                    
                if (_isBuildingBlock == false)
                {
                        _isInValidTrigger = false;
                }
                    
            }
            


            if (_isInValidTrigger && _isBuildingBlock && !blockPlaced)
            {

                if (Input.GetKeyDown(KeyCode.F))
                {
                    _firstPressTime = 0;
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
        }

        [ServerRpc(RequireOwnership = false)]
        public void ConfirmPlacementServerRpc(ServerRpcParams rpcParams = default)
        {
            ulong clienId = rpcParams.Receive.SenderClientId;
            
            if (!_playersConfirmed.Contains(clienId))
            {
                _playersConfirmed.Add(clienId);
                
                _confirmCounter = _playersConfirmed.Count;
                _textUI.text = "Press F to Place" + _confirmCounter;
                
                
                if (_playersConfirmed.Count == 2 && _firstPressTime <= _timeWindow)
                {
                    _placementConfirmed = true;
                    PlaceBlockClientRpc();
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
}