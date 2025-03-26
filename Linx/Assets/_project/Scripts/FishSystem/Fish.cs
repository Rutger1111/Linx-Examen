using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
namespace FishSystem
{
    public abstract class Fish : MonoBehaviour
    {
        [SerializeField] private float timer = 1;
        
        public bool caught = false;
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

        protected void Update()
        {
            if(timer - Time.deltaTime <= 0){
                timer = 1;
                CheckState();
            }
            else{
                timer -= Time.deltaTime;
            }
            if (caught == false){
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
                }            
            }
        }
        public virtual void CheckState(){
            if(Vector3.Distance(transform.position, bait.transform.position) < alluredDistance && state == EStates.Roaming){
                bool rolled = P_Roll(50);
                state = rolled == true ? EStates.Allured : EStates.Roaming;
                
                if (state == EStates.Allured){
                    return;
                }
            }
            if(state == EStates.Allured){
                state = P_Roll(50) == true ? EStates.Allured : EStates.Roaming;
            }
        }
        protected bool P_Roll(int percentageChance){
            return Random.Range(0, 100) <= percentageChance;
        }
    }
}