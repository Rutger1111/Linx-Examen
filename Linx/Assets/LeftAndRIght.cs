using _project.Scripts.PlanB;
using UnityEngine;

public class LeftAndRIght : Reeling
{
    public ulong horizontalPlayerId = 0;
    public ulong verticalPlayerId = 1;
    
    void Update()
    {
        if (OwnerClientId == 1)
        {
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
    }
}
