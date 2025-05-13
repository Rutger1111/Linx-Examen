using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectRotate : MonoBehaviour
{
    [SerializeField] private GameObject _hookPoint1;
    [SerializeField] private GameObject _hookPoint2;
    [SerializeField] private GameObject _box;

    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _followSpeed = 5f;

    private Gravity gravity;
    private void Start()
    {
        gravity = GetComponent<Gravity>();
    }

    void Update()
    {
        if (_hookPoint1 == null || _hookPoint2 == null || _box == null)
        {
            Debug.LogWarning("Missing reference in ObjectRotate! Check hookPoint1, hookPoint2, or box.");
            return;
        }
        
        FollowHook();
        RotateHook();
    }

    public void FollowHook()
    {
        Vector3 midpoint = (_hookPoint1.transform.position + _hookPoint2.transform.position) / 2f;
        
        _box.transform.position = Vector3.Lerp(_box.transform.position, midpoint, _followSpeed * Time.deltaTime);
    }
    
    public void RotateHook()
    {
        float yDiff = _hookPoint2.transform.position.y - _hookPoint1.transform.position.y;

        float targetAngle = yDiff * 10f;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, -targetAngle);
        
        _box.transform.rotation =
            Quaternion.Slerp(_box.transform.rotation, targetRotation,_rotationSpeed * Time.deltaTime);
    }
}
