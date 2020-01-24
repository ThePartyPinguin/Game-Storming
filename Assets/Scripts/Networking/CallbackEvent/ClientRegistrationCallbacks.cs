using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.Networking.Server;
using GameFrame.UnityHelpers.Networking;
using GameFrame.UnityHelpers.Networking.Message;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClientRegistrationCallbacks : MonoBehaviour
{
    [SerializeField]
    private ConnectedPlayers _connectedPlayers = default;

    public TopicEventCallbacks TopicEventCallbacks;

    private BlockGenerator blockGenerator;
    private UnityServerNetworkManager serverNetworkManager;

    private bool _topicReceived;
    private bool _waitingOnTopic;

    private List<ServerConnectedClient<NetworkEvent>> _connectedClientsWaitingToSendName;
    private LobbyManager _lobbyManager;

    void Start()
    {
        serverNetworkManager = UnityServerNetworkManager.Instance;
        _connectedClientsWaitingToSendName = new List<ServerConnectedClient<NetworkEvent>>();
    }


    public void OnClientConnected(ServerConnectedClient<NetworkEvent> client)
    {
        if (!_waitingOnTopic)
        {
            TopicEventCallbacks.FirstClientJoined(client);
            _connectedClientsWaitingToSendName.Add(client);
            StartCoroutine(WaitOnTopic());
            _waitingOnTopic = true;
        }
        else
        {
            if (!_topicReceived)
            {
                StartCoroutine(SendResponse(new EventOnlyNetworkMessage(NetworkEvent.SERVER_WAITING_ON_TOPIC), client));
                //client.SecureSendMessage();
                _connectedClientsWaitingToSendName.Add(client);
            }
            else
            {
                client.AddAllowedEvent(NetworkEvent.CLIENT_SEND_NAME);
                StartCoroutine(SendResponse(new EventOnlyNetworkMessage(NetworkEvent.SERVER_REQUEST_NAME), client));
            }
        }

    }

    private IEnumerator SendResponse(BaseNetworkMessage command, ServerConnectedClient<NetworkEvent> client)
    {
        yield return new WaitForSeconds(0.2f);
        client.SecureSendMessage(command);
    }

    public void ReceivedTopicRequestNames()
    {
        _topicReceived = true;
    }

    private IEnumerator WaitOnTopic()
    {
        while (!_topicReceived)
        {
            yield return new WaitForSeconds(2);
        }

        RequestAllPlayerNames();
    }

    private void RequestAllPlayerNames()
    {
        var message = new EventOnlyNetworkMessage(NetworkEvent.SERVER_REQUEST_NAME);
        foreach (var connectedClient in _connectedClientsWaitingToSendName)
        {
            connectedClient.AddAllowedEvent(NetworkEvent.CLIENT_SEND_NAME);
            StartCoroutine(SendResponse(message, connectedClient));
        }
    }

    //private IEnumerator RequestTopic(ServerConnectedClient<NetworkEvent> client)
    //{
    //    yield return new WaitForSeconds(0.5f);

    //    client.AddAllowedEvent(NetworkEvent.CLIENT_SEND_NAME);
    //    client.SecureSendMessage(new EventOnlyNetworkMessage(NetworkEvent.SERVER_REQUEST_NAME));
    //}


    public void OnReceiveClientName(StringNetworkMessage networkMessage, Guid clientId)
    {
        if (_lobbyManager == null)
            _lobbyManager = FindObjectOfType<LobbyManager>();

        Debug.Log("Name: " + networkMessage.Value + " received from: " + clientId);
        string pName = networkMessage.Value;
        Color pColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        Participant p = new Participant(clientId, pName, pColor);

        _lobbyManager.OnPlayerConnect(clientId, p.Name);

        //gameManager.RegisterNewParticipant(p);

        _connectedPlayers.AddConnectedPlayer(p);

        var client = serverNetworkManager.GameServer.GetClientById(clientId);

        client.RemoveAllowedEvent(NetworkEvent.CLIENT_SEND_NAME);
        client.AddAllowedEvent(NetworkEvent.CLIENT_SEND_IDEA);

        Debug.Log("Added client to gameManager");
        client.SecureSendMessage(new BaseNetworkMessage(NetworkEvent.SERVER_REGISTERED_CLIENT));
    }

    public void SpawnBlock(StringNetworkMessage message, Guid clientId)
    {
        if(blockGenerator == null)
            blockGenerator = FindObjectOfType<BlockGenerator>();

        Participant p = GameManager.Instance.GetParticipant(clientId);

        if(p == null)
            return;
        Debug.Log("Client: " + p.Name + " send a new idea: " + message.Value);
        blockGenerator.SpawnBlock(p, message.Value);
    }
}
