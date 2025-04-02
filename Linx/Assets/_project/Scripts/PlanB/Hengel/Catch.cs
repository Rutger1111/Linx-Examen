using FishSystem;
using UnityEngine;

namespace PlanB
{
    public class Catch : ICommand
   {
        [SerializeField] private int _maxFishCapacity = 100;
        private bool _isCatching;
        [SerializeField] private int _caughtFish = 0;

        public override void Invoke(Fish fish)
        {
            if (_caughtFish < _maxFishCapacity && _isCatching == false)
            {
                _caughtFish ++;
                fish.state = EStates.Caught;
                _isCatching = true;
            }
        }
    }
}