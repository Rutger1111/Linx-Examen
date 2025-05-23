using Unity.Netcode;
using UnityEngine;

public class GameStatesUI : MonoBehaviour
{
    [SerializeField] private GameObject _UIMultiplayer;
    [SerializeField] private GameObject _UIStopLooking;
    [SerializeField] private GameObject _stopServerUI;
    [SerializeField] private GameObject _logo;
    
    
   
    

    public void Quit()
    {
        Application.Quit();
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
        _logo.SetActive(true);
    }
    
}
