using Unity.Netcode;
using UnityEngine;

public class GameStatesUI : MonoBehaviour
{
    [SerializeField] private GameObject _UIMultiplayer;
    [SerializeField] private GameObject _UIStopLooking;
    [SerializeField] private GameObject _stopServerUI;
    
    
   
    
    public void Multiplayer()
    {
        TurnOnUI();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Back()
    {
        _UIMultiplayer.SetActive(false);
    }

    public void StopLook()
    {
        _UIStopLooking.SetActive(false);
        _UIMultiplayer.SetActive(true);
    }

    public void DisconnectHostAndClient()
    {
        NetworkManager.Singleton.Shutdown();
        
        _UIMultiplayer.SetActive(true);
        _stopServerUI.SetActive(false);
    }
    
    private void TurnOnUI()
    {
        _UIMultiplayer.SetActive(true);
    }
}
