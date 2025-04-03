using System;
using System.Security.Cryptography.X509Certificates;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.Rendering;
using Unity.Netcode;

public class CameraFollow : NetworkBehaviour
{
    [SerializeField] private GameObject _playerObject;
    public GameObject pier;

    public Vignette postposesting;
    
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _followSpeed2;
    [SerializeField] private float movementThreshold;
    [SerializeField] private float movementThreshold2 = 9f;
    [SerializeField] private float idleTimeThreshold = 3f; 
    [SerializeField] private float idleTimer;

    public float cameraZPosition = -9;

    public bool center;

    public float intensityValue;
    public float distanceToPier;

    public float distanceToPlayer;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, cameraZPosition);
        idleTimer = idleTimeThreshold;
        
        Volume volume = FindObjectOfType<Volume>();
        volume.profile.TryGet<Vignette>(out postposesting);
        
    }
    
    private NetworkVariable<Vector3> syncedCameraPosition = new NetworkVariable<Vector3>(
        Vector3.zero, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server
    );
    void Update()
    {

        idleTimer -= Time.deltaTime;

        if (center)
        {
            CenterCamera();
        }
        if (Vector3.Distance(_playerObject.transform.position, pier.transform.position) < 3)
        {
            cameraZPosition = -9f;
        }
        else if (Vector3.Distance(_playerObject.transform.position, pier.transform.position) > 3)
        {
            cameraZPosition = -5f;
        }
        
        distanceToPlayer = Vector3.Distance(new Vector3(transform.position.x, transform.position.y, 0),new Vector3(_playerObject.transform.position.x,_playerObject.transform.position.y, 0));

        if (distanceToPlayer < movementThreshold)
        {
            idleTimer = idleTimeThreshold;
            
            follow(_followSpeed);
        }
        else if (distanceToPlayer < movementThreshold2)
        {
            idleTimer = idleTimeThreshold;
         
            follow(_followSpeed2);
        }

        if (idleTimer <= 0)
        {
            center = true;
        }
        
        if (Mathf.Abs(_playerObject.transform.position.x - transform.position.x) <= 0.5f &&
            Mathf.Abs(_playerObject.transform.position.y - transform.position.y) <= 0.5f)
        {
            idleTimer = idleTimeThreshold;
            center = false;
        }
        
        distanceToPier = Vector3.Distance(transform.position, pier.transform.position);
    
        float minDistance = 10f;
        float maxDistance = 100f; 
        
        if (distanceToPier > minDistance)
        {
            if (distanceToPier <= maxDistance)
            {
                float targetIntensity = (distanceToPier - minDistance) * 0.1f; 
                intensityValue = Mathf.Lerp(postposesting.intensity.value, targetIntensity, Time.deltaTime * 0.1f);
                postposesting.intensity.Override(Mathf.Clamp(intensityValue, 0f, 0.35f));
            }
            else
            {
                
                float targetIntensity = (distanceToPier - minDistance) - 0.1f; 
                intensityValue = Mathf.Lerp(postposesting.intensity.value, targetIntensity, Time.deltaTime - 0.1f);
                postposesting.intensity.Override(Mathf.Clamp(intensityValue, 0f, 0.35f));
            }
        }
        else
        {
            postposesting.intensity.Override(0f);
        }
    }

    public void follow(float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(_playerObject.transform.position.x,
            _playerObject.transform.position.y,
            cameraZPosition), speed * Time.deltaTime);
    }
    
    
    
    public void CenterCamera()
    {
        transform.position = Vector3.Lerp(transform.position, 
            new Vector3(_playerObject.transform.position.x, _playerObject.transform.position.y, cameraZPosition), _followSpeed * Time.deltaTime);
    }
}
