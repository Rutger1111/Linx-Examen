using UnityEngine;

public class FPrey : Fish
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = EStates.Roaming;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state){
            case (EStates.Roaming):
                roamingCommand.Invoke(this);
                break;
        }
    }
}
