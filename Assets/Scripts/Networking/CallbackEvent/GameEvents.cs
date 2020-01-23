using System.Collections;
using System.Collections.Generic;
using GameFrame.UnityHelpers.Networking;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private UnityServerNetworkManager serverNetworkManager;

    void Start()
    {
        serverNetworkManager = UnityServerNetworkManager.Instance;
    }

    public void SendStartGameEvent()
    {
        serverNetworkManager.BroadcastMessage(new EventOnlyNetworkMessage(NetworkEvent.SERVER_START_GAME));
    }
}
