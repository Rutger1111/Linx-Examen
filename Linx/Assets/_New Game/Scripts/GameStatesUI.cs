using Unity.Netcode;
using UnityEngine;

namespace _New_Game.Scripts
{
    public class GameStatesUI : MonoBehaviour
    {
        [SerializeField] private GameObject uiMultiplayer;
        [SerializeField] private GameObject uiStopLooking;
        [SerializeField] private GameObject stopServerUI;

        public void Quit()
        {
            Application.Quit();
        }
    
        public void StopLook()
        {
            uiStopLooking.SetActive(false);
            uiMultiplayer.SetActive(true);
        }

        public void DisconnectHostAndClient()
        {
            NetworkManager.Singleton.Shutdown();
        
            uiMultiplayer.SetActive(true);
            stopServerUI.SetActive(false);
        }
    
    }
}
