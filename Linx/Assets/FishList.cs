using System;
using System.Collections;
using System.Collections.Generic;
using PlanB;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Slider = UnityEngine.UI.Slider;

public class FishList : MonoBehaviour
{
    
    public GameObject caught, fail;

    public List<GameObject> caughtFish;

    public float Timer;

    private void Update()
    {
        Timer -= Time.deltaTime;

        foreach (var CF in caughtFish)
        {
            if (CF == null)
            {
                caughtFish.Remove(CF);
            } 
        }
        
        if (Timer <= 0)
        {
            caught.SetActive(false);
        }
        
        if (Timer <= 0)
        {
            fail.SetActive(false);
        }
    }

    public void CaughtFish()
    {
        caught.SetActive(true);

        Timer = 0.1f;
        
        Timer++;

        foreach (var CF in caughtFish)
        {
            Destroy(CF);
        }
        GameObject.Find("Hook").GetComponent<Catch>().isCatching = null;
    }

    public void FailedFish()
    {
        fail.SetActive(true);
        
        Timer = 0.1f;
        
        Timer++;
        
        foreach (var CF in caughtFish)
        {
            Destroy(CF);
        }
        GameObject.Find("Hook").GetComponent<Catch>().isCatching = null;
    }
    
    
}
