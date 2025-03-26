using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillCheck : MonoBehaviour
{
    public List<UnityEvent> _unityEvents = new List<UnityEvent>();

    public bool battle;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (battle)
        {
            battle = false;
            int randomNumber = Random.Range(0, _unityEvents.Count);

            for (int i = 0; i < _unityEvents.Count; i++)
            {
                _unityEvents[randomNumber].Invoke();   
            }
        }
            
        
    }
}
