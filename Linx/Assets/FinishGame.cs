using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class FinishGame : NetworkBehaviour
{

    public List<Snap> _snaps = new List<Snap>();


    void Update()
    {
        if (_snaps.TrueForAll(snap => snap.blockPlaced))
            {
                NetworkManager.Singleton.SceneManager.LoadScene("Won", LoadSceneMode.Single);
            }
        
    }
}
