using GameFrame.Networking.Client;
using GameFrame.Networking.Exception;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.Serialization;
using GameFrame.UnityHelpers.Networking.Message;
using GameFrame.UnityHelpers.Networking.Shared;
using System;
using System.Collections;
using System.Net;
using GameFrame.Networking.Messaging.MessageHandling;
using UnityEngine;
using UnityEngine.Events;

namespace GameFrame.UnityHelpers.Networking.Client
{
    public class UnityNetworkManager : MonoSingleton<UnityNetworkManager>
    {
        public string ipAddress;
        public int tcpPort;
        public int udpReceivePort;
        public int udpRemoteSendPort;
        public SerializationType serializationType;
        public bool useUdp;
        public bool connectOnStart;

        [Header("Connection events")]
        public ClientIdCallback OnConnected;
        public ClientIdCallback OnConnectFailed;
        public ClientIdCallback OnConnectionInterrupted;


        [Header("Instantiating")]
        public NetworkedGameObjectCollection NetworkedGameObjects;

        private NetworkedGameObjectsFactory _networkedGameObjectsFactory;
        
        public GameClient<NetworkEvent> GameClient { get; private set; }
        private ClientConnectionSettings<NetworkEvent> _connectionSettings;
        
        void Start()
        {
            SetDefaultSettings();

            OnConnected.AddListener((guid) => SetupNetworkInstantiating());

            if(connectOnStart)
                Connect();
        }

        public void SendSecureMessage(BaseNetworkMessage message)
        {
            GameClient?.SecureSendMessage(message);
        }

        #region Setup

        private void SetDefaultSettings()
        {
            ClientConnectionSettings<NetworkEvent> settings = new ClientConnectionSettings<NetworkEvent>();

            settings.ClientToServerHandshakeEvent = NetworkEvent.CLIENT_TO_SERVER_HANDSHAKE;
            settings.ServerToClientHandshakeEvent = NetworkEvent.SERVER_TO_CLIENT_HANDSHAKE;
            settings.ClientDisconnectEvent = NetworkEvent.CLIENT_DISCONNECT;
            settings.ServerDisconnectEvent = NetworkEvent.SERVER_DISCONNECT;

            settings.SerializationType = serializationType;
            settings.ServerIpAddress = ParseIpAddress();
            settings.TcpPort = tcpPort;

            if (useUdp)
            {
                settings.UseUdp = useUdp;

                settings.UdpRemoteSendPort = udpRemoteSendPort;
                settings.UdpReceivePort = udpReceivePort;
            }

            _connectionSettings = settings;
        }
        public void SetSettings(ClientConnectionSettings<NetworkEvent> settings)
        {
            _connectionSettings = settings;
        }

        private IPAddress ParseIpAddress()
        {
            string ipAddress = this.ipAddress;
            if (this.ipAddress.Equals("localhost", StringComparison.OrdinalIgnoreCase))
                ipAddress = "127.0.0.1";

            if (IPAddress.TryParse(ipAddress, out var address))
            {
                return address;
            }
            throw new InvalidIpAddressException("The given ipAddress: " + this.ipAddress + " is not valid");
        }


        #endregion

        #region Connection
        public void Connect()
        {
            if (GameClient != null && GameClient.IsConnected)
                return;

            StartCoroutine(ConnectCoRoutine());
        }

        public void Disconnect()
        {
            GameClient?.SecureSendMessage(new ClientDisconnectMessage<NetworkEvent>(NetworkEvent.CLIENT_DISCONNECT));
        }

        public bool IsConnected()
        {
            if (GameClient == null)
                return false;

            return GameClient.IsConnected;
        }

        private IEnumerator ConnectCoRoutine()
        {
            yield return new WaitForSeconds(1f);

            Debug.Log("Setting up connection to server");
            
            if (GameClient == null)
            {
                GameClient = new GameClient<NetworkEvent>(_connectionSettings);

                GameClient.OnConnectionSuccess = (guid) =>
                {
                    ASyncToSynchronousCallbackHandler.Instance.QueueCallbackToHandle(() => OnConnected?.Invoke(guid));
                };
                
                GameClient.OnConnectionFailed += () =>
                {
                    ASyncToSynchronousCallbackHandler.Instance.QueueCallbackToHandle(() => OnConnectFailed?.Invoke(Guid.Empty));
                };
                GameClient.OnConnectionLost += () =>
                {
                    ASyncToSynchronousCallbackHandler.Instance.QueueCallbackToHandle(() => OnConnectionInterrupted?.Invoke(Guid.Empty));
                };
            }
            GameClient.Connect();
        }


        #endregion

        #region CallbackHandling

        [Serializable]
        public class ClientIdCallback : UnityEvent<Guid>
        {

        }

        void OnApplicationQuit()
        {
            GameClient?.Stop();
            Debug.Log("quit");
        }

        #endregion

        #region Instantiating

        private void SetupNetworkInstantiating()
        {
            Debug.Log("Connected, setting up instantiating ability");
            _networkedGameObjectsFactory = NetworkedGameObjectsFactory.Instance;

            _networkedGameObjectsFactory.SetupOnProduceCallback((o) => SendSecureMessage(new ClientInstantiateRequestMessage(NetworkEvent.CLIENT_INSTANTIATE_REQUEST, o.ObjectTypeId, o.NetworkId)));

            NetworkEventCallbackDatabase<NetworkEvent>.Instance.RegisterCallBack<ServerInstantiateResponse>(NetworkEvent.SERVER_INSTANTIATE_RESPONSE, 
                (response, connectorId) =>
                {
                    _networkedGameObjectsFactory.UpdateNetworkObjectNetworkId(response.ClientAssignedNetworkId, response.ServerAssignedNetworkId);
                });
        }

        #endregion
    }
}
