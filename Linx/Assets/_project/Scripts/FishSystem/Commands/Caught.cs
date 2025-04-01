using UnityEngine;
using UnityEngine.UIElements;
namespace FishSystem
{

    public class Caught : ICommand
    {
        bool _gotWaypoint = false;
        Vector3 _waypoint;

        public override void Invoke(Fish fish)
        {
            transform.position = fish.bait.transform.position;   
        }
    }
}