using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.PlanB.DdrGame
{
    public class Arrow : MonoBehaviour
    {
        private GameObject _canvas;
        private const float Speed = 2;

        [SerializeField] private Image destination;

        private void Start()
        {
            _canvas = GameObject.Find("Canvas");
            transform.position = new Vector3(/*960 + */destination.transform.position.x, -1000, 0);
            transform.SetParent(_canvas.transform);
        }

        void Update()
        {
            transform.position += Vector3.up * Speed;
            if (Mathf.Abs(transform.position.y - destination.transform.position.y) >= 50f)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Destroy(gameObject);
                }
            }
            
            Destroy(gameObject, 25);
        }
    }
}
