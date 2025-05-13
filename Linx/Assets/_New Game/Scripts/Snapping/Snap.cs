using System;
using FishSystem;
using ParrelSync.NonCore;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Snap : ICommand
{
    [SerializeField] private Material _myMaterial;
    public bool _isBuildingBlock = true;
    public int placed;
    public int isPickedUp;

    public GameObject UIplace;
    private Vector3 _pos;
    private quaternion _rot;
    private Vector3 _hook1Pos;
    private quaternion _hook1Rot;
    private Vector3 _hook2Pos;
    private quaternion _hook2Rot;
    public SnapPosition _snapPosition;
    public GameObject _hookObject1;
    public GameObject _hookObject2;
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
    void OnTriggerStay(Collider other)
    {
        if(placed >= 1){
            transform.position = _pos;
            transform.rotation = _rot;
            _hookObject1.transform.position = _hook1Pos;
            _hookObject1.transform.rotation = _hook1Rot;
            _hookObject1.transform.position = _hook2Pos;
            _hookObject1.transform.rotation = _hook2Rot;
        }
        if (other.gameObject.tag == "BuildPosition")
        {
            _snapPosition = other.GetComponent<SnapPosition>();
            
            Debug.Log(_snapPosition.gameObject);
            
            if (_snapPosition.hasObjectsInHere == false)
            {
                if (_isBuildingBlock && Input.GetKeyDown(KeyCode.F))
                {
                    print("fuck");
                    Invoke(other);
                    _isBuildingBlock = false;
                    _snapPosition.setTrue(true);
                    placed ++;
                }

                if (_isBuildingBlock)
                {
                    _myMaterial.color = Color.green;
                    UIplace.SetActive(true);
                }
                else
                {
                    _myMaterial.color = Color.yellow;
                    UIplace.SetActive(false);
                }
            }
        }
        
    }
    void OnTriggerExit(Collider other)
    {
        //placed --;
        _isBuildingBlock = true;
        _myMaterial.color = Color.yellow;
        UIplace.SetActive(false);
    }
    public override void Invoke(Fish fish)
    {
        throw new System.NotImplementedException();
    }
    public override void Invoke(Collider col)
    {
        if(GetComponent<Snap>().isPickedUp > 0){
            
            print("werktteeee");
            transform.rotation = new quaternion(col.transform.rotation.x,col.transform.rotation.y,col.transform.rotation.z,0);
            _pos = col.transform.position;
            _rot = new quaternion(col.gameObject.transform.rotation.x ,col.gameObject.transform.rotation.y,col.gameObject.transform.rotation.z,col.transform.rotation.w);
            _hook1Pos = _hookObject1.transform.position;
            _hook1Rot = _hookObject1.transform.rotation;
            _hook2Pos = _hookObject1.transform.position;
            _hook2Rot = _hookObject1.transform.rotation;
                    
        }
    }

}
