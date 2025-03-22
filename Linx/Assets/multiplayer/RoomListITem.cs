using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListITem : MonoBehaviour
{
    public TMP_Text Text;

    public RoomInfo info;
    
    public void setup(RoomInfo _info)
    {
        info = _info;
        Text.text = _info.Name;
    }

    public void onclick()
    {
        connectToServers.instance.joinRoom(info);
    }
}
