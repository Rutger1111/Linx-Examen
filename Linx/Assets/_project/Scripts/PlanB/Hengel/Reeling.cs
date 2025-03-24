using UnityEngine;

namespace _project.Scripts.PlanB
{
    public class Reeling : MonoBehaviour
    {
        private GameObject _hook;
        private GameObject _fishingRod;
        
        [SerializeField] private float lineLength;
        [SerializeField] private float distance;
        [SerializeField] private float resistance;

        public int weightOfFish;

        private LineRenderer _lineRenderer;

        private void Start()
        {
            _hook = GameObject.Find("HookStartLine");
            _fishingRod = GameObject.Find("LineStartPoint");
            _lineRenderer = FindAnyObjectByType<LineRenderer>();
        }

        void Update()
        {
            _lineRenderer.SetPosition(0, _fishingRod.transform.position);
            _lineRenderer.SetPosition(1, _hook.transform.position);
        
            MaxLineLength();
            ResistanceCalculation();

            if (Input.GetKey(KeyCode.W))
            {
                MoveToRod();
                //_hook.transform.position += new Vector3(0, 10, 0) * Time.deltaTime;
            }
        
            if (Input.GetKey(KeyCode.S))
            {
                _hook.transform.position -= new Vector3(0, resistance, 0) * Time.deltaTime;
            }
        }

        private void MoveToRod()
        {
            _hook.transform.position = Vector3.Lerp(_hook.transform.position, _fishingRod.transform.position, 0.01f);
        }

        private void MaxLineLength()
        {
            var rodPosition = _fishingRod.transform.position;
            distance = Vector3.Distance(_hook.transform.position, rodPosition);

            if (distance > lineLength)
            {
                
                Vector3 fromOriginToObject = _hook.transform.position - rodPosition;
                fromOriginToObject *= lineLength / distance;
                _hook.transform.position = rodPosition + fromOriginToObject;
            }
        }
        //doesn't work.
        protected void ResistanceCalculation()
        {
            resistance = distance + _hook.transform.position.y;
        }
    }
}
