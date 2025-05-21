using _New_Game.Scripts;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Distance : NetworkBehaviour
{
    public GameObject _cookie;
    public GameObject _redHook, _greenHook;

    public float _maxDistance;

    public PickUp _assignedPickUp;
    
    public NetworkObject _cookieNetObj;

    public int _lifes = 0;

    public PickUp picker;
    private void Start()
    {
        _cookieNetObj = _cookie.GetComponent<NetworkObject>();
    }

    void Update()
    {
        

        
        if (Vector3.Distance(_redHook.transform.position, _cookie.transform.position) > _maxDistance)
        {
            AssignPickerFromHook(_redHook);
        }
        
        
        if (Vector3.Distance(_greenHook.transform.position, _cookie.transform.position) > _maxDistance)
        {
            AssignPickerFromHook(_greenHook);
        }

        if (_lifes >= 3)
        {
            Debug.Log("blub");
        }
        
        print(_lifes);
    }

    void AssignPickerFromHook(GameObject hook)
    {
        
        picker = hook.GetComponentInParent<PickUp>();

        if (picker != null && _cookieNetObj != null)
        {
            print("distance");
            
            picker.Drop(_cookieNetObj.NetworkObjectId);
            _assignedPickUp = picker;
            _lifes += 1;
        }
    }
}
