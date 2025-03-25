using System.Buffers.Text;
using UnityEngine;
namespace FishSystem
{
    public class FPrey : Fish
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            state = EStates.Roaming;
        }

        // Update is called once per frame
        new void Update()
        {
            base.Update(); 
        }
    }
}
