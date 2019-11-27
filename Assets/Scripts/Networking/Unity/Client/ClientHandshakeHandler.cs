using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandshakeHandler : MonoBehaviour
{
    public void OnHandshakeReceive(EventOnlyNetworkMessage message)
    {
        Debug.Log("Received handshake");

        if (message.MessageEventType == NetworkEvent.SERVER_TO_CLIENT_HANDSHAKE)
        {
            UnityNetworkManager.Instance.NetworkConnector.SendMessage(new EventOnlyNetworkMessage(NetworkEvent.CLIENT_TO_SERVER_HANDSHAKE));
            StartCoroutine(TestStringMessage());
        }
    }

    private IEnumerator TestStringMessage()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Sending string message");
        UnityNetworkManager.Instance.NetworkConnector.SendMessage(new StringNetworkMessage(NetworkEvent.CLIENT_TO_SERVER_HANDSHAKE, "Test string"));
    }
}
