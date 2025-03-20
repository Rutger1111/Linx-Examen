using Unity.VisualScripting;
using UnityEngine;

public class Roaming : MonoBehaviour, ICommand
{
    bool gotWaypoint = false;
    Vector3 waypoint;
    public void Invoke(Fish fish)
    {
        if (gotWaypoint){
            MoveToWaypoint();
        }
        else{
            ChooseWaypoint(fish.fishmaxHeight, fish.fishmaxLow, fish.speed);
        }
    }
    private void ChooseWaypoint(float maxHeight, float MaxLow, float speed){
        
    }
    private void MoveToWaypoint(){

    }
}
