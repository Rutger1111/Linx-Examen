using UnityEngine;
namespace FishSystem
{
    public class Allured : ICommand
    {
        private Fish _thisFish;
        public override void Invoke(Fish fish)
        {
            _thisFish = fish;
            MoveToBait(fish.speed, fish.bait);
        }
        
        private void MoveToBait(float speed, GameObject bait)
        {
            Vector3 position = bait.transform.position;
            transform.position = Vector3.MoveTowards(transform.position ,position, speed * Time.deltaTime) ;
            Vector3 diff = position - transform.position;
            float angle = Mathf.Atan2(diff.y, diff.x);
            angle *= Mathf.Rad2Deg;
            
            if (angle >= -90  && angle <= 90){
                GetComponent<SpriteRenderer>().flipY = false;
            }
            else{
                GetComponent<SpriteRenderer>().flipY = true;
            }
            
            transform.rotation = Quaternion.Euler(0, 0, angle);
            if(Vector3.Distance(transform.position, position) < 0.1f){
                _thisFish.state = EStates.Caught;
            }
        }
    }
}