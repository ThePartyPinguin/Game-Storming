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
    public BlockGenerator BlockGenerator;

    private GameManager gameManager;
    private UnityServerNetworkManager serverNetworkManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        serverNetworkManager = UnityServerNetworkManager.Instance;
    }


    public void OnClientConnected(ServerConnectedClient<NetworkEvent> client)
    {
        Debug.Log("Added CLIENT_SEND_NAME to allowed events");
        client.AddAllowedEvent(NetworkEvent.CLIENT_SEND_NAME);
    }

    public void OnReceiveClientName(StringNetworkMessage networkMessage, Guid clientId)
    {
        Debug.Log("Name: " + networkMessage.Value + " received from: " + clientId);
        string pName = networkMessage.Value;
        Color pColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        Participant p = new Participant(clientId, pName, pColor);

        gameManager.RegisterNewParticipant(p);

        var client = serverNetworkManager.GameServer.GetClientById(clientId);

        client.RemoveAllowedEvent(NetworkEvent.CLIENT_SEND_NAME);
        client.AddAllowedEvent(NetworkEvent.CLIENT_SEND_IDEA);

        Debug.Log("Added client to gameManager");
        client.SecureSendMessage(new BaseNetworkMessage(NetworkEvent.SERVER_REGISTERED_CLIENT));
    }

    public void SpawnBlock(StringNetworkMessage message, Guid clientId)
    {
        Participant p = GameManager.Instance.GetParticipant(clientId);

        if(p == null)
            return;
        Debug.Log("Client: " + p.Name + " send a new idea: " + message.Value);
        BlockGenerator.SpawnBlock(p, message.Value);
    }
}
