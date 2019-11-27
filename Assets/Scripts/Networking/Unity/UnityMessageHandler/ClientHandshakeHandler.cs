using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandshakeHandler : MonoBehaviour
{
    public void OnConnected()
    {
        Debug.Log("Connected");
    }

    public void OnConnectFailed()
    {
        Debug.Log("Connected");
    }

    public void OnHandshakeReceive(EventOnlyNetworkMessage message)
    {
        if (message.MessageEventType == NetworkEvent.SERVER_TO_CLIENT_HANDSHAKE)
        {
            Debug.Log("Received handshake");
        }
    }
}
