using UnityEngine;

public class LobbyButtonUI : MonoBehaviour
{
    public string lobbyId;
    public string playerId;
    
    public void Setup(string playerid, string id)
    {
        lobbyId = id;
        playerId = playerid;
    }

    public void OnClickJoin()
    {
        print("check");
        
        FindObjectOfType<lobbytest>().joinLobby(lobbyId, playerId);
    }
}
