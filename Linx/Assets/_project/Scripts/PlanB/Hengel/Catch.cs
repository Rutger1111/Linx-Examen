using FishSystem;
using UnityEngine;

namespace PlanB
{
    public class Catch : ICommand
   {
        public GameObject isCatching;
        [SerializeField] private int _maxFishCapacity = 1;
        [SerializeField] private int _caughtFish = 0;

        public override void Invoke(Fish fish)
        {
            if (_caughtFish < _maxFishCapacity && isCatching == null)
            {
                isCatching = fish.gameObject;
                _caughtFish ++;
                fish.state = EStates.Caught;
            }
            else{
                fish.state = _caughtFish < _maxFishCapacity ? EStates.Caught : EStates.Roaming;
            }
        }
    }
}