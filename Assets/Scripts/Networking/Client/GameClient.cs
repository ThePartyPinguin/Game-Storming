using System;
using System.Threading;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.Networking.NetworkConnector;

namespace GameFrame.Networking.Client
{
    internal class GameClient<TEnum> where TEnum : Enum
    {
        public bool IsConnected => _isConnected;
        public Guid ClientId => _clientId;

        private Guid _clientId;
        private bool _isConnected;
        public Action<Guid> OnConnectionSuccess{ get; set;  }
        public Action OnConnectionFailed { get; set;  }
        public Action OnConnectionLost { get; set;  }

        private NetworkConnector<TEnum> _networkConnector;
        private readonly ClientConnectionSettings<TEnum> _connectionSettings;

        public GameClient(ClientConnectionSettings<TEnum> connectionSettings)
        {
            _connectionSettings = connectionSettings;
            NetworkEventCallbackDatabase<TEnum>.Instance.RegisterCallBack<ServerToClientHandshakeResponse<TEnum>>(connectionSettings.ServerToClientHandshakeEvent, OnReceiveHandShakeResponse);
            NetworkEventCallbackDatabase<TEnum>.Instance.RegisterCallBack<ServerDisconnectMessage<TEnum>>(connectionSettings.ServerDisconnectEvent, OnReceiveServerDisconnect);
        }

        private void OnReceiveHandShakeResponse(ServerToClientHandshakeResponse<TEnum> message, Guid connectorId)
        {
            if (!message.Accepted)
            {
                _isConnected = false;
                OnConnectionFailed?.Invoke();
                _networkConnector.Stop();
                return;
            }

            if(_connectionSettings.UseUdp)
                _networkConnector.StartUdp();

            _isConnected = true;
            _clientId = message.ClientId;
            OnConnectionSuccess?.Invoke(message.ClientId);
        }

        private void OnReceiveServerDisconnect(ServerDisconnectMessage<TEnum> message, Guid connectorId)
        {
            _isConnected = false;
            OnConnectionLost?.Invoke();
            StopWithoutSendingEvent();
        }

        private void StopWithoutSendingEvent()
        {
            _networkConnector.Stop();
        }

        public void Connect()
        {
            _networkConnector = new NetworkConnector<TEnum>(_connectionSettings.ServerIpAddress, _connectionSettings.TcpPort, _connectionSettings.UdpRemoteSendPort, _connectionSettings.UdpReceivePort);
            _networkConnector.Setup(_connectionSettings.SerializationType);
            _networkConnector.SetupCallbacks(OnConnectionFailed, OnConnectionLost);

            _networkConnector.Connect();

            _networkConnector.StartTcp();

            SecureSendMessage(new ClientToServerHandshakeRequest<TEnum>(_connectionSettings.ClientToServerHandshakeEvent));
        }

        /// <summary>
        /// Send a message securely using tcp
        /// </summary>
        /// <param name="message">message to be send</param>
        public void SecureSendMessage(NetworkMessage<TEnum> message)
        {
            _networkConnector.SecureSendMessage(message);
        }

        /// <summary>
        /// Send a message without guarantee of arrival using udp
        /// </summary>
        /// <param name="message">message to be send</param>
        public void SendMessage(NetworkMessage<TEnum> message)
        {
            _networkConnector.SendMessage(message);
        }

        public void Stop()
        {
            _isConnected = false;
            SecureSendMessage(new ClientDisconnectMessage<TEnum>(_connectionSettings.ClientDisconnectEvent));
            Thread.Sleep(200);
            _networkConnector.Stop();
        }
    }
}
