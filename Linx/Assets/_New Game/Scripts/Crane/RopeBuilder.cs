using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _New_Game.Scripts.Crane
{
    public class RopeBuilder : MonoBehaviour
    {
        [SerializeField] private GameObject segmentPrefab;     // Rope segment prefab
        [SerializeField] private int segmentCount = 10;        // Number of segments
        [SerializeField] private float segmentSpacing = 0.5f;  // Distance between segments
        [SerializeField] private Transform anchorPoint;        // Where the rope starts
        [SerializeField] private GameObject magnetPrefab;      // Magnet prefab to instantiate

        private List<GameObject> segments = new List<GameObject>();

        void Start()
        {
            anchorPoint.GetComponent<Rigidbody>().isKinematic = true;
            BuildRope();
            SpawnAndAttachMagnet();
        }
        void Update()
        {
            anchorPoint.GetComponent<Rigidbody>().isKinematic = true;
        }
        private void BuildRope()
        {
            GameObject previous = anchorPoint.gameObject;

            for (int i = 0; i < segmentCount; i++)
            {
                GameObject lastSegment = this.gameObject;
                if(segments.Count > 0){
                    lastSegment = segments[segments.Count - 1];
                }
                Vector3 spawnPos = anchorPoint.position + Vector3.down * segmentSpacing * (i + 1);
                GameObject segment = Instantiate(segmentPrefab, spawnPos, Quaternion.identity);

                segment.transform.SetParent(anchorPoint);

                Rigidbody rb = segment.GetComponent<Rigidbody>();
                ConfigurableJoint joint = segment.GetComponent<ConfigurableJoint>();

                joint.connectedBody = previous.GetComponent<Rigidbody>();

                joint.xMotion = ConfigurableJointMotion.Limited;
                joint.yMotion = ConfigurableJointMotion.Limited;
                joint.zMotion = ConfigurableJointMotion.Limited;

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
            
                lastSegment.GetComponent<ConfigurableJoint>().connectedBody = segment.GetComponent<Rigidbody>();
                if(segments.Count == 0){
                    segment.GetComponent<Rigidbody>().isKinematic = false;
                }
                segment.GetComponent<Rigidbody>().isKinematic = false;
                segments.Add(segment);
                previous = segment;
            }
        }

        private void SpawnAndAttachMagnet()
        {
            if (magnetPrefab == null || segments.Count == 0) return;

            GameObject lastSegment = segments[segments.Count - 1];
            Vector3 spawnPos = lastSegment.transform.position + Vector3.down * segmentSpacing;

            GameObject magnet = Instantiate(magnetPrefab, spawnPos, Quaternion.identity, anchorPoint);
            transform.parent.parent.parent.parent.parent.GetComponent<UpPick>()._pickUpPosition = magnet;
            Rigidbody magnetRb = magnet.GetComponent<Rigidbody>();
            if (magnetRb == null)
            {
                magnetRb = magnet.AddComponent<Rigidbody>();
            }

            magnetRb.mass = 1f;

            lastSegment.GetComponent<ConfigurableJoint>().connectedBody = magnet.GetComponent<Rigidbody>();
        }
    }
}