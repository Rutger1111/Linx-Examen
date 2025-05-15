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
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
    
    void OnTriggerStay(Collider other)
    {
        if(placed >= 1){

            transform.rotation = new Quaternion(0,0,0,0);
            transform.position = _pos;
            // transform.parent.rotation.eulerAngles.Set(_rot.x,_rot.y,_rot.z);
            print("komtookhier");
            // _hookObject1.transform.position = _hook1Pos;
            // _hookObject1.transform.rotation = _hook1Rot;
            // _hookObject1.transform.position = _hook2Pos;
            // _hookObject1.transform.rotation = _hook2Rot;
        }
        if (other.gameObject.tag == "BuildPosition")
        {
            _snapPosition = other.GetComponent<SnapPosition>();
            
            
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
            }
        }
        
    }
    void OnTriggerExit(Collider other)
    {
        //placed --;
        _isBuildingBlock = true;
        UIplace.SetActive(false);
    }
    public override void Invoke(Fish fish)
    {
        throw new System.NotImplementedException();
    }
    public override void Invoke(Collider col)
    {
        if(GetComponent<Snap>().isPickedUp > 0){
            print(transform.parent.name);
            transform.parent.rotation = col.transform.rotation;
            transform.parent.position = col.gameObject.transform.position;

            _pos = col.gameObject.transform.position;
            _hookObject1.SetActive(false);
            _hookObject2.SetActive(false);
            print("wordtgeroepen");
        }
    }

}