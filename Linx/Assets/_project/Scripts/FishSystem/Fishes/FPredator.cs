using System.Buffers.Text;
using Unity.Services.Matchmaker.Models;
using UnityEngine;
namespace FishSystem
{
    public class FPredator : Fish
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        new void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        new void Update()
        {
            base.Update(); 
        }

        public override void CheckState(){
            print("ben aangeroepen" + Vector3.Distance(transform.position, bait.transform.position) );

            if(Vector3.Distance(transform.position, bait.transform.position) < alluredDistance && state == EStates.Roaming){
                state = P_Roll(25) == true ? EStates.Hunting : EStates.Roaming;
                if(state == EStates.Hunting)
                {
                    return;
                }
            }
            if(state == EStates.Hunting){
                state = P_Roll(75) == true ? EStates.Hunting : EStates.Roaming;
            }
        }
    }
}
