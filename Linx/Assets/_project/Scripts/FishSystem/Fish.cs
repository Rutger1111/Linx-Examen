using UnityEngine;

public abstract class Fish : MonoBehaviour
{
    public EStates state;
    public float fishmaxHeight;
    public float fishmaxLow;
    public float fishmaxLeft;
    public float fishmaxRight;
    public float speed;

    public ICommand roamingCommand;
    public ICommand alluredCommand;

}
