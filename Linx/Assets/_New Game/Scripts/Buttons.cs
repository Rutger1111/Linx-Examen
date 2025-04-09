using System;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject UIMultiplayer;

    private void Start()
    {
        back();
    }


    public void singlePlayer()
    {
        print("Start Single Player");
    }
    
    public void multiplayer()
    {
        UIMultiplayer.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void back()
    {
        startScreen.SetActive(true);
        UIMultiplayer.SetActive(false);
    }
}
