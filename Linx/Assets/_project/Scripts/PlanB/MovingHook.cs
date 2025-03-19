using UnityEngine;

namespace _project.Scripts.PlanB
{
    public class MovingHook : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.position -= new Vector3(1 * Time.deltaTime, 0, 0);
            }
        
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(1 * Time.deltaTime, 0, 0);
            }
        }
    }
}
