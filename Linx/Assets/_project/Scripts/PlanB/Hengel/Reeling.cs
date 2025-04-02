using UnityEngine;

namespace _project.Scripts.PlanB.Hengel
{
    public class Reeling : MonoBehaviour
    {
        [SerializeField] private GameObject hook;
        [SerializeField] private GameObject fishingRod;

        [SerializeField] private float lineLength;
        [SerializeField] private float resistance;
        [SerializeField] private float reelSpeed;

        [SerializeField] private LineRenderer _lineRenderer;
        private float _distance;

        private void Awake()
        {
            // Cache components to avoid expensive runtime lookups
            if (!hook) hook = GameObject.FindWithTag("Hook");
            if (!fishingRod) fishingRod = GameObject.FindWithTag("Rod");

            //_lineRenderer = GetComponentInChildren<LineRenderer>();
            _lineRenderer = GetComponentInChildren<LineRenderer>();
            if (!_lineRenderer)
            {
                Debug.LogError("LineRenderer component missing on the GameObject.");
            }
        }

        private void Update()
        {
            UpdateLineRenderer();
            ApplyMaxLineLength();
            CalculateResistance();

            HandleInput();
        }

        private void UpdateLineRenderer()
        {
            if (!_lineRenderer || !hook || !fishingRod) return;

            _lineRenderer.SetPosition(0, fishingRod.transform.position);
            _lineRenderer.SetPosition(1, hook.transform.position);
        }

        private void HandleInput()
        {
            if (Input.GetKey(KeyCode.W))
            {
                MoveToRod();
            }

            if (Input.GetKey(KeyCode.S))
            {
                LowerHook();
            }
        }

        private void MoveToRod()
        {
            if (hook)
            {
                //hook.transform.position = Vector3.Lerp(hook.transform.position, fishingRod.transform.position, Time.deltaTime * reelSpeed);
                _distance -= reelSpeed * Time.deltaTime;
            }
        }

        private void LowerHook()
        {
            if (hook)
            {
                //hook.transform.position -= Vector3.up * (resistance * Time.deltaTime);
                _distance += reelSpeed * Time.deltaTime;
            }
        }

        private void ApplyMaxLineLength()
        {
            if (!hook || !fishingRod) return;

            _distance = Vector3.Distance(hook.transform.position, fishingRod.transform.position);

            if (_distance > lineLength)
            {
                var position = fishingRod.transform.position;
                Vector3 direction = (hook.transform.position - position).normalized;
                hook.transform.position = position + direction * lineLength;
            }
        }

        private void CalculateResistance()
        {
            resistance = Mathf.Max(1f, _distance * 0.1f + hook.transform.position.y * 0.05f);
        }
    }
}
