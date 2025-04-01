using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.PlanB.DdrGame
{
    public class DdrGame : MonoBehaviour
    {
        //public GameObject canvas;
        public Image clone;
        
        public Image pijltjeUp;
        public Image pijltjeDown;
        public Image pijltjeLinks;
        public Image pijltjeRechts;
        public Image pijltjeUpDes;
        public Image pijltjeDownDes;
        public Image pijltjeLinksDes;
        public Image pijltjeRechtsDes;

        private float Timer;

        public float time;
        public GameObject _canvas;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Timer = time;
            _canvas = GameObject.Find("Canvas");
        }

        // Update is called once per frame
        void Update()
        {
            SpawnArrowUp();
            if (Input.GetKeyDown(KeyCode.P))
            {
                Instantiate(pijltjeDownDes);
                Instantiate(pijltjeUpDes);
                Instantiate(pijltjeLinksDes);
                Instantiate(pijltjeRechtsDes);
            }
        }

        public void SpawnArrowUp()
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0f)
            {
                Instantiate(pijltjeUp);
                Instantiate(pijltjeDown);
                Instantiate(pijltjeLinks);
                Instantiate(pijltjeRechts);
                Timer = time;
            }
            //clone.transform.SetParent(canvas.transform);
        }
    }
}
