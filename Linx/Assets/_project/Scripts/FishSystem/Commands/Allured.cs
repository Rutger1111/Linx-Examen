using PlanB;
using UnityEngine;
namespace FishSystem
{
    public class Allured : ICommand
    {
        [SerializeField] private int _swimRadius;
        private bool _gotWaypoint = false;
        private Vector3 _waypoint;
        private Fish _thisFish;
        public override void Invoke(Fish fish)
        {
            _thisFish = fish;
            if (_gotWaypoint){
                MoveToWaypoint(fish.speed);
            }
            else{
                ChooseWaypoint(fish); 
            }
            
        }
        private void ChooseWaypoint(Fish fish){
            Vector3 bait = fish.bait.transform.position;
            _waypoint = new Vector3(Random.insideUnitCircle.x * _swimRadius + bait.x,Random.insideUnitCircle.x * _swimRadius + bait.y,transform.position.z);
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