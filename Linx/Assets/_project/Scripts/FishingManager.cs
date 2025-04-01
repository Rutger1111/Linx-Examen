using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Slider = UnityEngine.UI.Slider;

public class FishingManager : MonoBehaviour
{
    
    [SerializeField] private GameObject _caughtFishUI, _failedFishUI;

    [SerializeField] private List<GameObject> caughtFishList;

    [SerializeField] private float Timer;

    private void Update()
    {
        Timer -= Time.deltaTime;

        foreach (var fish in caughtFishList)
        {
            if (fish == null)
            {
                caughtFishList.Remove(fish);
            } 
        }
        
        if (Timer <= 0)
        {
            _caughtFishUI.SetActive(false);
            _failedFishUI.SetActive(false);
        }
    }

    public void HandleFishCaught()
    {
        _caughtFishUI.SetActive(true);
        Timer = 0.1f;
        
        Timer++;

        foreach (var fish in caughtFishList)
        {
            Destroy(fish);
        }
    }

    public void HandleFishFailed()
    {
        _failedFishUI.SetActive(true);
        Timer = 0.1f;
        
        Timer++;
        
        foreach (var fish in caughtFishList)
        {
            Destroy(fish);
        }
    }
    
    
}
