using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject _playerObject;
    [FormerlySerializedAs("speed")] [SerializeField] private float _followSpeed;
    [FormerlySerializedAs("distanceForMovement")] [SerializeField] private float movementThreshold;
    [FormerlySerializedAs("notmovingTime")] [SerializeField] private float idleTimeThreshold = 3f; 
    [FormerlySerializedAs("notmoving")] [SerializeField] private float idleTimer;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
        idleTimer = idleTimeThreshold;
    }

    void Update()
    {
        Vector3 direction = _playerObject.transform.position - transform.position;

        idleTimer -= Time.deltaTime;
        
        if (Mathf.Abs(direction.x) > movementThreshold || Mathf.Abs(direction.y) > movementThreshold)
        {
            idleTimer = idleTimeThreshold;
            
            
            transform.position = Vector3.MoveTowards(transform.position, 
                new Vector3(_playerObject.transform.position.x, _playerObject.transform.position.y, transform.position.z), _followSpeed * Time.deltaTime);
            
            
        }
        else if (idleTimer <= 0)
        {
            transform.position = Vector3.Lerp(transform.position, 
                    new Vector3(_playerObject.transform.position.x, _playerObject.transform.position.y, transform.position.z), _followSpeed * Time.deltaTime);
        }

        if (Mathf.Abs(direction.x) > movementThreshold * 1.5 || Mathf.Abs(direction.y) > movementThreshold * 1.5)
        {
            transform.position = Vector3.MoveTowards(transform.position, 
                new Vector3(_playerObject.transform.position.x, _playerObject.transform.position.y, transform.position.z), _followSpeed + 10 * Time.deltaTime);
        }
    }
}
