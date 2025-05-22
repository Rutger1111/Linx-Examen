using Unity.Netcode;
using UnityEngine;

namespace _New_Game.Scripts
{
    public class UI : NetworkBehaviour
    {
        [SerializeField] private GameObject pauseScreen;
    

        public void ResumeGame()
        {
            pauseScreen.SetActive(false);
        }
    
        public void QuitGame()
        {
            NetworkManager.Singleton.Shutdown();
        }
    }
}
