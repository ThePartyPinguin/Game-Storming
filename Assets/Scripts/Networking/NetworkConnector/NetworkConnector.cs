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

        private NetworkReceiver<TEnum> _receiver;
        private NetworkSender<TEnum> _sender;

        private IPAddress _ipAddress;
        private int _port;

        private TcpClient _tcpClient;

        private INetworkMessageSerializer<TEnum> _networkMessageSerializer;
        private bool _setupComplete;

        public NetworkConnector(IPAddress ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;
        }

        /// <summary>
        /// Setup the sender and receiver
        /// </summary>
        /// <param name="onMessageReceived">a callback that get's called when a new message is received and deserialized'</param>
        /// <param name="serializationType">The used serializationType</param>
        /// <param name="onConnectionLost">A callback that gets invoked when the send or receiver catches an error</param>
        public void Setup(Action<NetworkMessage<TEnum>> onMessageReceived, SerializationType serializationType, Action onConnectionLost)
        {
            if(_setupComplete)
                throw new AlreadySetupExcpetion("The: " + this.GetType() + " has already been setup");

            SetupTcpClient();

            switch (serializationType)
            {
                case SerializationType.JSON:
                    _networkMessageSerializer = new JsonNetworkMessageSerializer<TEnum>();
                    break;
            }

            _receiver = new TcpNetworkReceiver<TEnum>(_tcpClient, onConnectionLost);

            _receiver.RegisterNewMessageHandler(new NetworkMessageDeserializer<TEnum>(onMessageReceived, _networkMessageSerializer));

            _sender = new TcpNetworkSender<TEnum>(_networkMessageSerializer, _tcpClient, onConnectionLost);

            _setupComplete = true;
        }

        private void SetupTcpClient()
        {
            Debug.Log(_ipAddress);
            
        }

        /// <summary>
        /// Try to connect the tcp client, if succeed, start the message receiver
        /// </summary>
        /// <param name="onConnected">A callback that get's invoked when the tcpClient successfully connects to a host</param>
        /// <param name="onConnectFailed">A callback that get's invoked when the tcpClient throws an error when trying to connect to the given host</param>
        public void Connect(Action onConnected, Action onConnectFailed)
        {
            if(!_setupComplete)
                throw new NotSetupCorrectlyException("The " + this.GetType() + " was not setup correct, please run setup() before connecting");

            try
            {
                _tcpClient = new TcpClient();
                _tcpClient.Connect(_ipAddress, _port);
                _receiver.StartReceiving();

                onConnected?.Invoke();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                onConnectFailed?.Invoke();
            }
        }

        public void Close()
        {
            try
            {
                Debug.Log("Trying to close port");

                _tcpClient.GetStream().Flush();
                _tcpClient.GetStream().Dispose();
                _tcpClient.GetStream().Close();

            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
            finally
            {
                _tcpClient.Dispose();
                Debug.Log("Connection closed");
            }
        }
    }
}
