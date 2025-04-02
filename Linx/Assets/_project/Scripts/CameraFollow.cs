using System;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject _playerObject;
    public GameObject pier;
    
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _followSpeed2;
    [SerializeField] private float movementThreshold;
    [SerializeField] private float movementThreshold2 = 9f;
    [SerializeField] private float idleTimeThreshold = 3f; 
    [SerializeField] private float idleTimer;

    public float cameraZPosition = -9;

    public bool center;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, cameraZPosition);
        idleTimer = idleTimeThreshold;
    }

    void Update()
    {
        Vector3 direction = _playerObject.transform.position - transform.position;
        
        idleTimer -= Time.deltaTime;

        if (center == true)
        {
            CenterCamera();
        }

        if (Mathf.Approximately(_playerObject.transform.position.x, transform.position.x) &&
            Mathf.Approximately(_playerObject.transform.position.y, transform.position.y))
        {
            idleTimer = idleTimeThreshold;
            
            center = false;
        }
        
        if (idleTimer <= 0)
        {
            center = true;
        }
        else 
        {
            
            
            if (Mathf.Abs(direction.x) > movementThreshold2 || Mathf.Abs(direction.y) > movementThreshold2)
            {
                idleTimer = idleTimeThreshold;
                print("duck");
                transform.position = Vector3.MoveTowards(transform.position, 
                    new Vector3(_playerObject.transform.position.x, _playerObject.transform.position.y, cameraZPosition), _followSpeed2 * Time.deltaTime);
            }
            else if (Mathf.Abs(direction.x) > movementThreshold || Mathf.Abs(direction.y) > movementThreshold)
            {
                idleTimer = idleTimeThreshold;
                print("fuck");
                transform.position = Vector3.MoveTowards(transform.position, 
                    new Vector3(_playerObject.transform.position.x, _playerObject.transform.position.y, cameraZPosition), _followSpeed * Time.deltaTime);
            }
            
        }
        
        if (Vector3.Distance(_playerObject.transform.position, pier.transform.position) < 3)
        {
            cameraZPosition = -9f;
        }
        else if (Vector3.Distance(_playerObject.transform.position, pier.transform.position) > 3)
        {
            cameraZPosition = -5f;
        }
    }

    public void CenterCamera()
    {
        transform.position = Vector3.Lerp(transform.position, 
            new Vector3(_playerObject.transform.position.x, _playerObject.transform.position.y, cameraZPosition), _followSpeed * Time.deltaTime);
    }
}
