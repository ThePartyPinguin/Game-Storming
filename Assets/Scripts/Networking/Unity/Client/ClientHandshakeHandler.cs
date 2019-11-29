using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandshakeHandler : MonoBehaviour
{
    public void OnHandshakeReceive(EventOnlyNetworkMessage message)
    {
        if (message.MessageEventType == NetworkEvent.SERVER_TO_CLIENT_HANDSHAKE)
        {
            Debug.Log("Received handshake");

            StartCoroutine(TestStringMessage());
        }
    }

    private IEnumerator TestStringMessage()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Sending string message");
        UnityNetworkManager.Instance.NetworkConnector.SendMessage(new StringNetworkMessage(NetworkEvent.CLIENT_SEND_TEST_STRING, "Test string"));
    }
}
