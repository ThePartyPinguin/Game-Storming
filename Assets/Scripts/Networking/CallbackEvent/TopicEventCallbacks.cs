using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFrame.Networking.Server;
using GameFrame.UnityHelpers.Networking;
using UnityEngine;


public class TopicEventCallbacks : MonoBehaviour
{
    public ClientRegistrationCallbacks ClientRegistrationCallbacks;

    private TopicTracker topicTracker;
    void Start()
    {
        topicTracker = TopicTracker.Instance;
    }

    public void FirstClientJoined(ServerConnectedClient<NetworkEvent> client)
    {
        StartCoroutine(RequestTopic(client));
    }

    private IEnumerator RequestTopic(ServerConnectedClient<NetworkEvent> client)
    {
        yield return new WaitForSeconds(0.5f);

        client.AddAllowedEvent(NetworkEvent.CLIENT_SEND_TOPIC);
        client.SecureSendMessage(new EventOnlyNetworkMessage(NetworkEvent.SERVER_REQUEST_TOPIC));
    }


    public void OnReceiveTopic(StringNetworkMessage message, Guid clientId)
    {
        topicTracker.SetTopic(message.Value);

        ClientRegistrationCallbacks.ReceivedTopicRequestNames();
    }
}

