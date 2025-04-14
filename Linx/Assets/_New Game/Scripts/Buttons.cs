using UnityEngine;

public class Buttons : MonoBehaviour
{
    [SerializeField] private GameObject _UIStartScreen;
    [SerializeField] private GameObject _UIMultiplayer;

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
        turnOn();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void back()
    {
        _UIStartScreen.SetActive(true);
        _UIMultiplayer.SetActive(false);
    }

    private void turnOn()
    {
        _UIMultiplayer.SetActive(true);
        _UIStartScreen.SetActive(false);
    }
}
