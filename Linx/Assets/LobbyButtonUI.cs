using UnityEngine;

public class LobbyButtonUI : MonoBehaviour
{
    public string lobbyId;
    public string playerId;
    
    public void Setup(string playerid, string id)
    {
        lobbyId = id;
    }

    public void OnClickJoin()
    {
        print("check");

        FindObjectOfType<MultiplayerSystem>().joinLobby(lobbyId);
    }
}
