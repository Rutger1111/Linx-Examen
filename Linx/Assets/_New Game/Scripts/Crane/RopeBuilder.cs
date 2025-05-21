using System.Collections.Generic;
using UnityEngine;

namespace _New_Game.Scripts.Crane
{
    public class RopeBuilder : MonoBehaviour
    {
        public GameObject segmentPrefab;     // Rope segment prefab
        public int segmentCount = 10;        // Number of segments
        public float segmentSpacing = 0.5f;  // Distance between segments
        public Transform anchorPoint;        // Where the rope starts
        public GameObject magnetPrefab;      // Magnet prefab to instantiate

        public Transform MagnetParent;

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
            transform.parent.parent.parent.parent.parent.GetComponent<PickUp>().pickUpPosition = magnet;
            Rigidbody magnetRb = magnet.GetComponent<Rigidbody>();
            if (magnetRb == null)
            {
                magnetRb = magnet.AddComponent<Rigidbody>();
            }

            magnetRb.mass = 1f;

            // Add and configure joint
            ConfigurableJoint joint = null;

            lastSegment.GetComponent<ConfigurableJoint>().connectedBody = magnet.GetComponent<Rigidbody>();
            //joint.autoConfigureConnectedAnchor = true;

            joint.xMotion = ConfigurableJointMotion.Limited;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Limited;

            joint.angularXMotion = ConfigurableJointMotion.Free;
            joint.angularYMotion = ConfigurableJointMotion.Free;
            joint.angularZMotion = ConfigurableJointMotion.Free;

            SoftJointLimit limit = joint.linearLimit;
            limit.limit = segmentSpacing;
            joint.linearLimit = limit;
        }
    }
}