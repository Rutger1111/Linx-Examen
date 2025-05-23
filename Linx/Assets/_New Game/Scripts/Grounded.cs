using System;
<<<<<<< HEAD
using _New_Game.Scripts;
=======
>>>>>>> backup
using _New_Game.Scripts.Crane;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    [SerializeField] private Movement _movement;
    
    
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _movement.MovementDisable(false);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
<<<<<<< HEAD
=======
            print("hit");
>>>>>>> backup
            _movement.MovementDisable(true);
        }
    }
}
