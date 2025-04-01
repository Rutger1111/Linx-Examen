using FishSystem;
using UnityEngine;
namespace PlanB
{
    public class Catch : ICommand
   {
        [SerializeField] private int _maxFishCapacity = 1;
        [SerializeField] private int _caughtFish = 0;

        public override void Invoke(Fish fish)
        {
            if (_caughtFish < _maxFishCapacity)
            {
                _caughtFish ++;
                fish.state = EStates.Caught;
            }
        }
    }
}