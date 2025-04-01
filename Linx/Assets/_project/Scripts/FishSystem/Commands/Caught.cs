using System;
using UnityEngine;
using UnityEngine.UIElements;
namespace FishSystem
{

    public class Caught : ICommand
    {
        bool _gotWaypoint = false;
        Vector3 _waypoint;

        
        
        public SkillCheck _skillCheck;
        public FishList _fishList;

        private bool isbool;

        private void Start()
        {
            _skillCheck = GameObject.Find("EventSystem").GetComponent<SkillCheck>();
            _fishList = GameObject.Find("EventSystem").GetComponent<FishList>();
        }

        public override void Invoke(Fish fish)
        {
            transform.position = fish.bait.transform.position;

            if (Vector3.Distance(transform.position, fish.bait.transform.position) < 0.001 && isbool == false)
            {
                _skillCheck.MiniGame();
                
                isbool = true;
                
                _fishList.caughtFish.Add(fish.gameObject);
            }
        }
    }
}