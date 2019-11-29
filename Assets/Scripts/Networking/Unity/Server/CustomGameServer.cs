using System;
using System.Collections.Generic;
using System.Net.Sockets;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.Networking.NetworkConnector;
using GameFrame.Networking.Serialization;
using GameFrame.Networking.Server;
using UnityEngine;

class CustomGameServer : GameServer<NetworkEvent>
{
    private Dictionary<int, NetworkConnector<NetworkEvent>> _connectedClients;

    private readonly SerializationType _serializationType;
    //private readonly INetworkMessageHandler<NetworkMessage<NetworkEvent>, NetworkEvent> _onClientAcceptedNewMessageHandler;

    public CustomGameServer(int port, int maxConnectedClients, SerializationType serializationType) : base(port)
    {
        _serializationType = serializationType;
        _connectedClients = new Dictionary<int, NetworkConnector<NetworkEvent>>();

        NetworkEventCallbackDatabase<NetworkEvent>.Instance.RegisterCallBack<EventOnlyNetworkMessage>(NetworkEvent.CLIENT_TO_SERVER_HANDSHAKE, OnReceiveHandshakeRequest);
    }

    public void StartServer()
    {
        Start(OnClientConnect);
    }

    public void StopServer()
    {
        Stop();
    }

    private void OnClientConnect(TcpClient client)
    {
        var connector = new NetworkConnector<NetworkEvent>(client);
        connector.Setup(_serializationType);
        connector.SetupCallbacks(() => { }, OnConnectionLost);
        connector.Start();
    }

    private void OnConnectionLost(NetworkConnector<NetworkEvent> connector)
    {

    }

    private void OnReceiveHandshakeRequest(EventOnlyNetworkMessage message, NetworkConnector<NetworkEvent> connector)
    {
        if (message.MessageEventType == NetworkEvent.CLIENT_TO_SERVER_HANDSHAKE)
        {
            Debug.Log("Received client handshake request");
            int clientId = _connectedClients.Count;
            _connectedClients.Add(clientId, connector);
            connector.SendMessage(new HandshakeServerResponseMessage(NetworkEvent.SERVER_TO_CLIENT_HANDSHAKE, clientId));
        }
    }

    private void OnClientDisconnect(NetworkConnector<NetworkEvent> connector)
    {

    }
}
