using System;
using System.Net;
using System.Net.Sockets;
using GameFrame.Networking.Exception;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.Networking.Serialization;
using UnityEngine;

namespace GameFrame.Networking.NetworkConnector
{
    public class NetworkConnector<TEnum> where TEnum : Enum
    {
        private IPAddress _ipAddress;
        private int _port;

        private NetworkReceiver<TEnum> _receiver;
        private NetworkSender<TEnum> _sender;

        private TcpClient _tcpClient;

        private INetworkMessageSerializer<TEnum> _networkMessageSerializer;
        private bool _setupComplete;

        private Action<NetworkConnector<TEnum>> _onConnectionLost;
        private Action _onConnected;
        private Action _onConnectionFailed;
        private Action<NetworkMessage<TEnum>> _onMessageReceived;


        public NetworkConnector(IPAddress ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;
        }

        public NetworkConnector(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
        }

        /// <summary>
        /// Setup the sender and receiver
        /// </summary>
        /// <param name="onMessageReceived">this callback get's called when the message deserializer has has deserialized a new message</param>
        /// <param name="serializationType">The used serializationType</param>
        /// <param name="onConnectionLost">This callback get's called when the receiver or sender throws an exception</param>
        public void Setup(SerializationType serializationType)
        {
            if(_setupComplete)
                throw new AlreadySetupExcpetion("The: " + this.GetType() + " has already been setup");

            switch (serializationType)
            {
                case SerializationType.JSON:
                    _networkMessageSerializer = new JsonNetworkMessageSerializer<TEnum>();
                    break;
            }
            
            _setupComplete = true;
        }

        public void SetupCallbacks(Action<NetworkMessage<TEnum>> onMessageReceived, Action onConnected, Action onConnectionFailed, Action<NetworkConnector<TEnum>> onConnectionLost)
        {
            _onMessageReceived = onMessageReceived;
            _onConnectionLost = onConnectionLost;
            _onConnected = onConnected;
            _onConnectionFailed = onConnectionFailed;
        }

        public void Start()
        {
            _receiver = new TcpNetworkReceiver<TEnum>(new NetworkMessageDeserializer<TEnum>(OnMessageReceived, _networkMessageSerializer), _tcpClient, OnConnectionLost);
            _sender = new TcpNetworkSender<TEnum>(_networkMessageSerializer, _tcpClient, OnConnectionLost);
            _receiver.StartReceiving();
        }

        private void OnConnectionLost()
        {
            _onConnectionLost?.Invoke(this);
        }

        private void OnMessageReceived(NetworkMessage<TEnum> message)
        {
            _onMessageReceived?.Invoke(message);
        }
        /// <summary>
        /// Try to connect the tcp client, if succeed, start the message receiver
        /// </summary>
        public void Connect()
        {
            if (!_setupComplete)
                throw new NotSetupCorrectlyException("The " + this.GetType() + " was not setup correct, please run setup() before connecting");

            try
            {
                _tcpClient = new TcpClient();
                _tcpClient.Connect(_ipAddress, _port);


                _onConnected?.Invoke();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                _onConnectionFailed?.Invoke();
                throw;
            }
        }

        public void SendMessage(NetworkMessage<TEnum> message)
        {
            if (!_setupComplete)
                throw new NotSetupCorrectlyException("The: " + this.GetType() + " has not been setup yet");

            _sender.QueueNewMessageToSend(message);
        }

        public void Stop()
        {
            try
            {
                _receiver?.StopReceiving();
                _sender?.Stop();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                if(_tcpClient != null)
                    _tcpClient.Dispose();

                Debug.Log("Client stopped");
            }
        }
    }
}
