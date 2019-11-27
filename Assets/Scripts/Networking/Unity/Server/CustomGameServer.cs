using System;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.Networking.NetworkConnector;
using GameFrame.Networking.Serialization;
using GameFrame.Networking.Server;
using UnityEngine;

class CustomGameServer : GameServer<NetworkEvent>
{
    private Dictionary<int, NetworkConnector<NetworkEvent>> _connectedClients;

    private ServerHandshakeHandler _handshakeHandler;
    private readonly SerializationType _serializationType;
    private readonly INetworkMessageHandler<NetworkMessage<NetworkEvent>, NetworkEvent> _onClientAcceptedNewMessageHandler;

    public CustomGameServer(int port, int maxConnectedClients, INetworkMessageHandler<NetworkMessage<NetworkEvent>, NetworkEvent>  newNetworkMessageHandler, SerializationType serializationType) : base(port)
    {
        _serializationType = serializationType;
        _onClientAcceptedNewMessageHandler = newNetworkMessageHandler;
        _connectedClients = new Dictionary<int, NetworkConnector<NetworkEvent>>();
    }

    public void StartServer()
    {
        if (_handshakeHandler == null)
            _handshakeHandler = new ServerHandshakeHandler(OnReceiveHandshakeResponse, OnClientDisconnect, _serializationType);

        Start(_handshakeHandler);
    }

    public void StopServer()
    {
        Stop();
    }

    private void OnReceiveHandshakeResponse(EventOnlyNetworkMessage message, NetworkConnector<NetworkEvent> connector)
    {
        Debug.LogError("[OnReceiveHandshakeResponse]: " + message.MessageEventType);
        if (message.MessageEventType == NetworkEvent.CLIENT_TO_SERVER_HANDSHAKE)
        {
            Debug.LogError("A new client has been accepted");
            int clientId = _connectedClients.Count;
            connector.SetupCallbacks(_onClientAcceptedNewMessageHandler.MessageHandled, () => {}, () => {}, OnClientDisconnect);
            _connectedClients.Add(clientId, connector);
        }
    }

    private void OnClientDisconnect(NetworkConnector<NetworkEvent> connector)
    {

    }
}
