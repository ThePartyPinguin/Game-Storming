using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.Networking.NetworkConnector;
using GameFrame.Networking.Serialization;
using UnityEngine;

class ServerHandshakeHandler : InitMonoBehaviour<SerializationType, Action<NetworkConnector<NetworkEvent>>>, IHandshakeHandler
{
    private Queue<NetworkConnector<NetworkEvent>> _connectorsToAccept;
    private SerializationType _serializationType;
    private readonly INetworkMessageHandler<NetworkMessage<NetworkEvent>, NetworkEvent> _onClientAcceptedMessageHandler;

    private bool _acceptCoRoutineRunning;

    private Action<NetworkConnector<NetworkEvent>> _onConnectionLost;

    public override void Init(SerializationType param, Action<NetworkConnector<NetworkEvent>> onConnectionLost)
    {
        _serializationType = param;
    }

    public void SendHandshakeToClient(NetworkConnector<NetworkEvent> connector)
    {
        connector.SendMessage(new EventOnlyNetworkMessage(NetworkEvent.SERVER_TO_CLIENT_HANDSHAKE));
    }


    public void AddClientToAccept(TcpClient client)
    {
        Debug.Log("Adding client to accept queue");
        var connector = new NetworkConnector<NetworkEvent>(client);
        connector.Setup(_serializationType);
        connector.SetupCallbacks(_onClientAcceptedMessageHandler.MessageHandled, () => { }, () => { }, OnConnectionLost);
        connector.Start();

        _connectorsToAccept.Enqueue(connector);

        if (!_acceptCoRoutineRunning)
        {
            _acceptCoRoutineRunning = true;
            StartCoroutine(AcceptCoRoutine());
        }
    }

    private void OnConnectionLost(NetworkConnector<NetworkEvent> connector)
    {
        _onConnectionLost?.Invoke(connector);
    }

    private IEnumerator AcceptCoRoutine()
    {
        while (_connectorsToAccept.Count > 0)
        {
            var connector = _connectorsToAccept.Dequeue();
            SendHandshakeToClient(connector);
            yield return new WaitForEndOfFrame();
        }
        _acceptCoRoutineRunning = false;
    }

}

