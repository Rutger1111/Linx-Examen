using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
namespace FishSystem
{
    public abstract class Fish : MonoBehaviour
    {
        [SerializeField] private float _timerReset = 1;
        [SerializeField] private float _timer = 1;        
        [SerializeField] private int alluredChance = 50;
        [SerializeField] private int roamingChance = 50;
        [SerializeField] private int biteChance = 2;
        
        public float alluredDistance = 5;
        public EStates state;
        public GameObject bait;

        public float fishmaxHeight;
        public float fishmaxLow;
        public float fishmaxLeft;
        public float fishmaxRight;
        public float speed;

        public ICommand roamingCommand;
        public ICommand alluredCommand;
        public ICommand huntingCommand;
        public ICommand biteCommand;
        public ICommand caught;

        protected void Start()
        {
            bait = GameObject.Find("Hook");
            state = EStates.Roaming;
        }

        protected void Update()
        {
            // timer to check the state each second
            if(_timer - Time.deltaTime <= 0){
                _timer = 1;
                CheckState();
            }
            else{
                _timer -= Time.deltaTime;
            }
            // Check what state it is in and call the state's function
            switch(state){
                case (EStates.Roaming):
                    roamingCommand.Invoke(this);
                    break;
                case (EStates.Allured):
                    alluredCommand.Invoke(this);
                    break;
                case (EStates.Hunting):
                    huntingCommand.Invoke(this);
                    break;
                case (EStates.Caught):
                    caught.Invoke(this);
                    break;
                case (EStates.Biting):
                    biteCommand.Invoke(this);
                    break;
            }            

        }
        // check whether it still has to be in its current state
        public virtual void CheckState()
        {
            // checks whether its close enaught and is roaming then checks whether it still has to be roaming
            if(Vector3.Distance(transform.position, bait.transform.position) < alluredDistance && state == EStates.Roaming){
                bool rolled = P_Roll(alluredChance);
                state = rolled == true ? EStates.Allured : EStates.Roaming;
                
                if (state == EStates.Allured){
                    return;
                }
            }
            // checks if its allured
            if(state == EStates.Allured){
                // rolls whether to make the fish roam, stay allured or bite the bait
                state = P_Roll(roamingChance) == true ? EStates.Roaming : EStates.Allured;
                if (state == EStates.Allured){
                    state = P_Roll(biteChance) == true ? EStates.Biting : EStates.Allured;
                }
                return;
            }
            //checks if the state is biting
            if(state == EStates.Biting){
                // rolls whether is has to stay in biting state
                state = P_Roll(roamingChance) == true ? EStates.Roaming : EStates.Biting;
            }
        }
        // function for rolling chances
        protected bool P_Roll(int percentageChance){
            // returns true if number is under percentagechance
            return Random.Range(0, 100) <= percentageChance;
        }
    }
}