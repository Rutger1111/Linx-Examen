using System;
using UnityEngine;
using System.Collections.Generic;

public class RopeBuilder : MonoBehaviour
{
    public GameObject segmentPrefab;     // Your rope segment prefab
    public int segmentCount = 10;        // How many links in the rope
    public float segmentSpacing = 0.5f;  // Distance between links
    public Transform anchorPoint;        // Starting point of the rope

    private List<GameObject> segments = new List<GameObject>();

    void Start()
    {
        GameObject previous = anchorPoint.gameObject;

        for (int i = 0; i < segmentCount; i++)
        {
            Vector3 spawnPos = anchorPoint.transform.position + Vector3.down * segmentSpacing * (i + 1);
            GameObject segment = Instantiate(segmentPrefab, spawnPos, Quaternion.identity);
            
            segment.transform.SetParent(anchorPoint);

            Rigidbody rb = segment.GetComponent<Rigidbody>();
            ConfigurableJoint joint = segment.GetComponent<ConfigurableJoint>();

            joint.connectedBody = previous.GetComponent<Rigidbody>();

            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Locked;

            SoftJointLimit limit = joint.linearLimit;
            limit.limit = segmentSpacing;
            joint.linearLimit = limit;

            joint.angularXMotion = ConfigurableJointMotion.Limited;
            joint.angularYMotion = ConfigurableJointMotion.Limited;
            joint.angularZMotion = ConfigurableJointMotion.Limited;

            SoftJointLimitSpring angularSpring = new SoftJointLimitSpring
            {
                spring = 0f,
                damper = 0f
            };
            joint.angularXLimitSpring = angularSpring;
            joint.angularYZLimitSpring = angularSpring;

            segments.Add(segment);
            previous = segment;
        }
    }
}