using UnityEngine;


namespace _project.Scripts.PlanB
{
    public class MovingHook : Reeling
    {
        void Update()
        {//player 1 should move these
            if (OwnerClientId == 0)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    transform.position -= new Vector3(10, 0, 0) * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    transform.position += new Vector3(10, 0, 0) * Time.deltaTime;
                }
            }

            ResistanceCalculation();
        }
    }
}
