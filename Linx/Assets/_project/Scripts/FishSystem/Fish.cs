using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Fish : MonoBehaviour
{
    public bool caught = false;
    private float timer = 1;
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
            }            
        }
    }
    private void CheckState(){
        print("ben aangeroepen" + Vector3.Distance(transform.position, bait.transform.position) );

        if(Vector3.Distance(transform.position, bait.transform.position) < alluredDistance && state == EStates.Roaming){
            bool rolled = Roll(50);
            state = rolled == true ? EStates.Allured : EStates.Roaming;
            if (rolled == true){
                return;
            }
        }
        if(state == EStates.Allured){
            state = Roll(50) == true ? EStates.Allured : EStates.Roaming;
        }
    }
    private bool Roll(int percentageChance){
        return Random.Range(0, 100) <= percentageChance;
    }
}
