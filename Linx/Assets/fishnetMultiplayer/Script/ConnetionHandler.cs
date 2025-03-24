using System;
using FishNet;
using FishNet.Transporting;
using UnityEngine;

public enum connectType
{
    Host,
    Client
}
public class ConnetionHandler : MonoBehaviour
{
    public connectType _connectType;

    public void OnDisable()
    {
        InstanceFinder.ClientManager.OnClientConnectionState -= onClientConnectionState;
    }

    public void OnEnable()
    {
        InstanceFinder.ClientManager.OnClientConnectionState += onClientConnectionState;
    }

    private void onClientConnectionState(ClientConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Stopped)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
    private void Start()
    {
        if (ParrelSync.ClonesManager.IsClone())
        {
            InstanceFinder.ClientManager.StartConnection();
        }
        else
        {
            if (_connectType == connectType.Host)
            {
                InstanceFinder.ServerManager.StartConnection();
                InstanceFinder.ClientManager.StartConnection();    
            }
            else
            {
                InstanceFinder.ClientManager.StartConnection();
            }
        }
        
        
    }
}
