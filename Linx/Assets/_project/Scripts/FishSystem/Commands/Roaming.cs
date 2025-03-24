using UnityEngine;
using UnityEngine.UIElements;

public class Roaming : ICommand
{
    bool gotWaypoint = false;
    Vector3 waypoint;

    public override void Invoke(Fish fish)
    {
        print(waypoint);
        if (gotWaypoint){
            MoveToWaypoint(fish.speed);
        }
        else{
            ChooseWaypoint(fish); 
        }
        
    }
    private void ChooseWaypoint(Fish fish){
        waypoint = new Vector3(Random.Range(fish.fishmaxLeft, fish.fishmaxRight),Random.Range(fish.fishmaxLow, fish.fishmaxHeight),transform.position.z);
        gotWaypoint = true;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(waypoint, 0.5f);
    }
    private void MoveToWaypoint(float speed){
        transform.position = Vector3.MoveTowards(transform.position ,waypoint, speed * Time.deltaTime) ;
        Vector3 diff = waypoint - transform.position;
        float angle = Mathf.Atan2(diff.y, diff.x);
        angle *= Mathf.Rad2Deg;
        print(angle);
        if (angle >= -90  && angle <= 90){
            GetComponent<SpriteRenderer>().flipY = false;
        }
        else{
            GetComponent<SpriteRenderer>().flipY = true;
        }
        transform.rotation = Quaternion.Euler(0, 0, angle);
        if(Vector3.Distance(transform.position, waypoint) < 0.1f){
            gotWaypoint = false;
        }
    }

   
}
