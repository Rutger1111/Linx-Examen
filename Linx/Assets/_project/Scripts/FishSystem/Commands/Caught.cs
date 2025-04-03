using System;
using PlanB;
using UnityEngine;
using UnityEngine.UIElements;
namespace FishSystem
{

    public class Caught : ICommand
    {
        bool _gotWaypoint = false;
        Vector3 _waypoint;

        
        
        public SkillCheck _skillCheck;
        public FishingManager _fishList;

        private bool isbool;

        private void Start()
        {
            _skillCheck = GameObject.Find("EventSystem").GetComponent<SkillCheck>();
            _fishList = GameObject.Find("EventSystem").GetComponent<FishingManager>();
        }

        public override void Invoke(Fish fish)
        {
            transform.position = fish.bait.transform.position;
            GameObject isCatching = fish.bait.GetComponent<Catch>().isCatching;
            if (Vector3.Distance(transform.position, fish.bait.transform.position) < 0.001 && isbool == false)
            {
                _skillCheck.MiniGame();
                
                isbool = true;
                
                _fishList._caughtFishList.Add(fish.gameObject);
                fish.bait.GetComponent<Catch>().Invoke(fish);
            }
        }
    }
}