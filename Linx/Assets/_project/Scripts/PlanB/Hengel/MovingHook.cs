using UnityEditor;
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
            Range();
            ResistanceCalculation();
        }

        private void Range()
        {
            Gizmos.DrawSphere(gameObject.transform.position, 5f);
        }
        
        private void OnDrawGizmos()
        {
            // Set the color with custom alpha.
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f); // Red with custom alpha

            // Draw the sphere.
            Gizmos.DrawSphere(transform.position, 5);

            // Draw wire sphere outline.
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, 5);
        }
        
        private void Catch()
        {
            
        }
    }
}
