using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.Networking.NetworkConnector;
using GameFrame.Networking.Serialization;
using UnityEngine;

namespace GameFrame.Networking.Server
{
    public abstract class HandshakeHandler<TEnum> where TEnum : Enum
    {
        private Queue<NetworkConnector<TEnum>> _connectorsToAccept;

        private Action<NetworkMessage<TEnum>, NetworkConnector<TEnum>> _onHandshakeReceived;
        private Action<NetworkConnector<TEnum>> _onConnectionLost;
        private SerializationType _serializationType;

        private Task _acceptTask;
        private bool _acceptTaskRunning;

        protected HandshakeHandler(Action<NetworkMessage<TEnum>, NetworkConnector<TEnum>> onHandshakeReceived, Action<NetworkConnector<TEnum>> onConnectionLost, SerializationType serializationType)
        {
            _onHandshakeReceived = onHandshakeReceived;
            _onConnectionLost = onConnectionLost;
            _serializationType = SerializationType.JSON;
            _connectorsToAccept = new Queue<NetworkConnector<TEnum>>();
        }

        public void AddClientToAccept(TcpClient client)
        {
            Debug.Log("Adding client to accept queue");
            var connector = new NetworkConnector<TEnum>(client);
            connector.Setup(_serializationType);
            connector.SetupCallbacks((message) => { OnHandshakeReceived(message, connector);}, () => {}, () => {}, OnConnectionLost);
            connector.Start();

            _connectorsToAccept.Enqueue(connector);

            if (!_acceptTaskRunning)
            {
                _acceptTaskRunning = true;
                _acceptTask = new Task(AcceptConnectors);
                _acceptTask.GetAwaiter().OnCompleted(OnAcceptTaskStop);
                _acceptTask.Start();
            }
        }

        private void OnHandshakeReceived(NetworkMessage<TEnum> message, NetworkConnector<TEnum> connector)
        {
            Debug.Log("ReceivedMessage");
            _onHandshakeReceived?.Invoke(message, connector);
        }

        private void OnConnectionLost(NetworkConnector<TEnum> connector)
        {
            _onConnectionLost?.Invoke(connector);
        }

        private void AcceptConnectors()
        {
            while (_connectorsToAccept.Count > 0)
            {
                var connector = _connectorsToAccept.Dequeue();
                SendHandshakeToClient(connector);
            }
        }
        
        private void OnAcceptTaskStop()
        {
            _acceptTaskRunning = false;
        }

        protected abstract void SendHandshakeToClient(NetworkConnector<TEnum> connector);
    }
}
