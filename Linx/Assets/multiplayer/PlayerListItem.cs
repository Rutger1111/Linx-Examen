using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    public TMP_Text text;
    
    private Player player;
    
    public void setup(Player _player)
    {
        player = _player;
        text.text = _player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
       
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
