using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _New_Game.Scripts
{
    public class StartGame : MonoBehaviour
    {
        [SerializeField] private string gameplayScene = "Multiplayer";
    
        public GameObject playersJoinedListPrefab;

        public GameObject parent2;
    

        private void Update()
        {
            PlayersJoined();
        }

        private void PlayersJoined()
        {
            try
            {
                foreach (Transform child in parent2.transform)
                {
                    Destroy(child.gameObject);
                }

               
                int connectedPlayersCount = NetworkManager.Singleton.ConnectedClientsList.Count;
                Debug.Log("Number of players in the lobby: " + connectedPlayersCount);

               
                foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
                {
                    Instantiate(playersJoinedListPrefab, parent2.transform);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        
        }

        public void startGame()
        {
            Debug.Log("StartGame called. IsServer: " + NetworkManager.Singleton.IsServer);

            if (NetworkManager.Singleton.IsServer)
            {
                Debug.Log("Loading scene: " + gameplayScene);
                NetworkManager.Singleton.SceneManager.LoadScene(gameplayScene, LoadSceneMode.Single);
            }
            else
            {
                Debug.Log("Client tried to start game — ignored.");
            }
        }
    

    
    
    }
}
