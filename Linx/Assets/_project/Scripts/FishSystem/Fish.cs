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

        public ICommand caught;
        protected void Start()
        {
            bait = GameObject.Find("Player2");
            state = EStates.Roaming;
        }
        protected void Update()
        {
            if(_timer - Time.deltaTime <= 0){
                _timer = 1;
                CheckState();
            }
            else{
                _timer -= Time.deltaTime;
            }
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
            }            

        }
        public virtual void CheckState(){
            if(Vector3.Distance(transform.position, bait.transform.position) < alluredDistance && state == EStates.Roaming){
                bool rolled = P_Roll(alluredChance);
                state = rolled == true ? EStates.Allured : EStates.Roaming;
                
                if (state == EStates.Allured){
                    return;
                }
            }
            if(state == EStates.Allured){
                state = P_Roll(roamingChance) == true ? EStates.Allured : EStates.Roaming;
            }
        }
        protected bool P_Roll(int percentageChance){
            return Random.Range(0, 100) <= percentageChance;
        }
    }
}