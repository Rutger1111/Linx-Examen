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
    private Quaternion _rot;
    private Vector3 _hook1Pos;
    private quaternion _hook1Rot;
    private Vector3 _hook2Pos;
    private quaternion _hook2Rot;
    private SnapPosition _snapPosition;
    [SerializeField] private GameObject _hookObject1;
    [SerializeField] private GameObject _hookObject2;
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
            Quaternion _colRot = col.gameObject.transform.rotation;
            print(transform.parent.name);

            transform.parent.position = col.gameObject.transform.position;
            transform.rotation.eulerAngles.Set(0, 0, 0);
            _pos = col.gameObject.transform.position;
            _rot = transform.rotation;
            _hookObject1.SetActive(false);
            _hookObject2.SetActive(false);
            print("wordtgeroepen");
            // _hook1Pos = _hookObject1.transform.position;
            // _hook1Rot = _hookObject1.transform.rotation;
            // _hook2Pos = _hookObject1.transform.position;
            // _hook2Rot = _hookObject1.transform.rotation;
            transform.parent.rotation = col.transform.rotation;
            Debug.Log("Euler na instellen: " + transform.rotation.eulerAngles);
            Debug.DrawRay(transform.position, transform.forward * 2, Color.red, 5f);
        }
    }

}
