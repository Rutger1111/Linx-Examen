using UnityEngine;

namespace _project.Scripts.PlanB
{
    public class Reeling : MonoBehaviour
    {
        private GameObject _hook;
        private GameObject _fishingRod;

        private float _resistance;
        public float maxDepth;

        private bool _reachedMaxDepth;

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
        
            ResistanceCalculation();

            if (Input.GetKey(KeyCode.W))
            {
                _hook.transform.position += new Vector3(0, _resistance, 0) * Time.deltaTime;
            }
        
            if (Input.GetKey(KeyCode.S) && !_reachedMaxDepth)
            {
                _hook.transform.position -= new Vector3(0, _resistance, 0) * Time.deltaTime;
            }
        }

        private void ResistanceCalculation()
        {
            _resistance = maxDepth + _hook.transform.position.y;

            _reachedMaxDepth = _resistance <= 2;
        }
    }
}
