using System;
using System.Collections;
using System.Collections.Generic;
using PlanB;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Slider = UnityEngine.UI.Slider;

public class FishingManager : NetworkBehaviour
{
    
    [SerializeField] private GameObject _caughtFishUI, _failedFishUI;

    [SerializeField] public List<GameObject> _caughtFishList;

    [SerializeField] private float _timer;

    private SkillCheck _skillCheck;

    private void Start()
    {
        _skillCheck = GetComponent<SkillCheck>();
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        foreach (var fish in _caughtFishList)
        {
            if (fish == null)
            {
                _caughtFishList.Remove(fish);
            } 
        }
        
        if (_timer <= 0)
        {
            _caughtFishUI.SetActive(false);
            _failedFishUI.SetActive(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void HandleFishCaughtServerRpc()
    {
        HandleFishCaughtClientRpc();
    }

    [ClientRpc]
    private void HandleFishCaughtClientRpc()
    {
        Debug.Log("KomtHier");
        GameObject.Find("Hook").GetComponent<Catch>().isCatching = null;
        GameObject.Find("Hook").GetComponent<Catch>()._caughtFish ++;
        _skillCheck.minigame = false;
        
        _caughtFishUI.SetActive(true);
        _timer = 0.1f;

        _timer++;
        
        _skillCheck.minigame = false;

        foreach (var fish in _caughtFishList)
        {
            Destroy(fish);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void HandleFishFailedServerRpc()
    {
        HandleFishFailedClientRpc();
    }

    [ClientRpc]
    private void HandleFishFailedClientRpc()
    {
        GameObject.Find("Hook").GetComponent<Catch>().isCatching = null;
        
        _skillCheck.minigame = false;
        
        _failedFishUI.SetActive(true);
        _timer = 0.1f;
        
        _timer++;
        
        foreach (var fish in _caughtFishList)
        {
            Destroy(fish);
        }
    }
    
    
}
