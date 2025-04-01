using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SkillCheck : MonoBehaviour
{
    [SerializeField] private List<UnityEvent> _unityEvents = new List<UnityEvent>();
    [SerializeField] private int randomNumber;

    

    public void MiniGame()
    {
        randomNumber = Random.Range(0, _unityEvents.Count);
        
        _unityEvents[randomNumber].Invoke();   
    }
}
