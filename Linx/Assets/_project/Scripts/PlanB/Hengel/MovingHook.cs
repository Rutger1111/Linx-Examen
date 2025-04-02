using _project.Scripts.PlanB.Hengel;
using UnityEngine;

namespace _project.Scripts.PlanB
{
    public class MovingHook : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.position -= new Vector3(10, 0, 0) * Time.deltaTime;
            }
        
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(10, 0, 0) * Time.deltaTime;
            }
            //ResistanceCalculation();
        }
    }
}
