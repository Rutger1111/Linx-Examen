using UnityEngine;
using UnityEngine.UIElements;
namespace FishSystem
{

    public class Roaming : ICommand
    {
        bool _gotWaypoint = false;
        Vector3 _waypoint;

        public override void Invoke(Fish fish)
        {
            if (_gotWaypoint){
                MoveToWaypoint(fish.speed);
            }
            else{
                ChooseWaypoint(fish); 
            }
            
        }

        private void ChooseWaypoint(Fish fish){
            _waypoint = new Vector3(Random.Range(fish.fishmaxLeft, fish.fishmaxRight),Random.Range(fish.fishmaxLow, fish.fishmaxHeight),transform.position.z);
            _gotWaypoint = true;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(_waypoint, 0.5f);
        }
        private void MoveToWaypoint(float speed){
            transform.position = Vector3.MoveTowards(transform.position ,_waypoint, speed * Time.deltaTime) ;
            Vector3 diff = _waypoint - transform.position;
            float angle = Mathf.Atan2(diff.y, diff.x);
            angle *= Mathf.Rad2Deg;
            
            if (angle >= -90  && angle <= 90){
                GetComponent<SpriteRenderer>().flipY = false;
            }
            else{
                GetComponent<SpriteRenderer>().flipY = true;
            }
            
            transform.rotation = Quaternion.Euler(0, 0, angle);
            if(Vector3.Distance(transform.position, _waypoint) < 0.1f){
                _gotWaypoint = false;
            }
        }    
    }
}