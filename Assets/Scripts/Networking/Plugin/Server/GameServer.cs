using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.Networking.NetworkConnector;
using GameFrame.Networking.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace GameFrame.Networking.Server
{
    public sealed class GameServer<TEnum> where TEnum : Enum
    {
        public IPAddress LocalIpAddress
        {
            get
            {
                if (_serverListener != null)
                   return _serverListener.LocalIpAddress;

                return IPAddress.Loopback;
            }
        }

        public int ListenPort
        {
            get
            {
                if (_serverListener != null)
                    return _serverListener.ListenPort;

                return 0;
            }
        }

        public int ConnectedCount
        {
            get
            {
                if (_connectedClients != null)
                    return _connectedClients.Count;
                return 0;
            }
        }

        public bool IsStarted { get; private set; }

        private readonly int _tcpPort;
        private readonly int _udpPort;

        private readonly Dictionary<Guid, ServerConnectedClient<TEnum>> _connectedClients;
        private readonly Dictionary<Guid, NetworkConnector<TEnum>> _connectorsToAccept;

        private ServerListener<TEnum> _serverListener;

        private readonly SerializationType _serializationType = default;

        private readonly ServerSettings<TEnum> _serverSettings;

        private Action<ServerConnectedClient<TEnum>> _onClientAccepted;

        public GameServer(ServerSettings<TEnum> serverSettings, Action<ServerConnectedClient<TEnum>> onClientAccepted)
        {
            _serverSettings = serverSettings;
            _onClientAccepted = onClientAccepted;

            _connectedClients = new Dictionary<Guid, ServerConnectedClient<TEnum>>();
            _connectorsToAccept = new Dictionary<Guid, NetworkConnector<TEnum>>();
        }

        public void StartServer()
        {
            if(IsStarted)
                return;
            if (_serverListener == null)
            {
                _serverListener = new ServerListener<TEnum>(_serverSettings.TcpPort);
                NetworkEventCallbackDatabase<TEnum>.Instance.RegisterCallBack<ClientToServerHandshakeRequest<TEnum>>(_serverSettings.ClientToServerHandshakeEvent, OnReceiveHandshakeRequest);
                NetworkEventCallbackDatabase<TEnum>.Instance.RegisterCallBack<ClientDisconnectMessage<TEnum>>(_serverSettings.ClientDisconnectEvent, OnClientDisconnect);
            }

            _serverListener.StartListener(OnClientConnect);
            IsStarted = true;
        }

        public void StopServer()
        {
            _serverListener.StopListener();

            SecureBroadcastMessage(new ServerDisconnectMessage<TEnum>(_serverSettings.ServerDisconnectEvent));

            Thread.Sleep(200);

            foreach (var client in _connectedClients.Values)
            {
                client.Stop();
            }

            try
            {
                if (NetworkConnector<TEnum>.UdpClient != null)
                {
                    NetworkConnector<TEnum>.UdpClient.Close();
                    NetworkConnector<TEnum>.UdpClient.Dispose();
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                Thread.Sleep(100);
                throw;
            }
        }

        public ServerConnectedClient<TEnum> GetClientById(Guid clientId)
        {
            if (_connectedClients.ContainsKey(clientId))
                return _connectedClients[clientId];

            return null;
        }

        private void RemoveConnection(Guid connectorId)
        {
            if (!_connectedClients.ContainsKey(connectorId))
                return;

            var client = _connectedClients[connectorId];

            client.Stop();

            _connectedClients.Remove(connectorId);
        }

        #region Callbacks

        private void OnClientConnect(TcpClient client)
        {
            Guid guid = Guid.NewGuid();

            var connector = new NetworkConnector<TEnum>(guid, client, _serverSettings.UdpRemoteSendPort, _serverSettings.UdpReceivePort);
            connector.Setup(_serializationType);
            connector.SetupCallbacks(() => { }, () => OnConnectionLost(guid));

            connector.AddAllowedEvent(_serverSettings.ClientToServerHandshakeEvent);
            connector.AddAllowedEvent(_serverSettings.ClientDisconnectEvent);

            connector.StartTcp();

            _connectorsToAccept.Add(guid, connector);
        }

        private void OnConnectionLost(Guid connectorId)
        {

        }

        private void OnReceiveHandshakeRequest(ClientToServerHandshakeRequest<TEnum> message, Guid connectorId)
        {
            if (message.MessageEventType.Equals(_serverSettings.ClientToServerHandshakeEvent))
            {
                var connector = _connectorsToAccept[connectorId];

                Console.WriteLine("New handshake request received");
                if (_connectedClients.Count >= _serverSettings.MaxConnectedClients)
                {
                    
                    connector.SecureSendMessage(new ServerToClientHandshakeResponse<TEnum>(_serverSettings.ServerToClientHandshakeEvent, false, Guid.Empty));
                    connector.Stop();
                    _connectorsToAccept.Remove(connectorId);
                    Console.WriteLine("Client was rejected, too many connections");

                    return;
                }

                _connectorsToAccept.Remove(connectorId);

                var client = new ServerConnectedClient<TEnum>(connectorId, connector);

                _connectedClients.Add(connectorId, client);

                if(_serverSettings.UseUdp)
                    connector.StartUdp();
                Debug.Log(_serverSettings.ServerToClientHandshakeEvent);
                connector.SecureSendMessage(new ServerToClientHandshakeResponse<TEnum>(_serverSettings.ServerToClientHandshakeEvent, true, connectorId));

                connector.RemoveAllowedEvent(_serverSettings.ClientToServerHandshakeEvent);

                _onClientAccepted?.Invoke(client);
            }
        }

        private void OnClientDisconnect(ClientDisconnectMessage<TEnum> message, Guid connectorId)
        {
            if (!_connectedClients.ContainsKey(connectorId) && message.MessageEventType.Equals(_serverSettings.ClientDisconnectEvent))
                return;

            Console.WriteLine("Client disconnect event received, removing client");

            RemoveConnection(connectorId);
        }

        #endregion
        
        #region Broadcast

        public void BroadcastMessage(NetworkMessage<TEnum> message)
        {
            if(!_serverSettings.UseUdp)
                throw new System.Exception("Server not initialized to use udp");

            foreach (var clientId in _connectedClients.Keys)
            {
                SendMessageToPlayer(clientId, message);
            }
        }

        public void SecureBroadcastMessage(NetworkMessage<TEnum> message)
        {
            foreach (var clientId in _connectedClients.Keys)
            {
                SecureSendMessageToPlayer(clientId, message);
            }
        }

        public void BroadcastMessage(NetworkMessage<TEnum> message, Guid excludePlayerId)
        {

            if (!_serverSettings.UseUdp)
                throw new System.Exception("Server not initialized to use udp");

            foreach (var clientId in _connectedClients.Keys)
            {
                if(clientId.Equals(excludePlayerId))
                    continue;

                SendMessageToPlayer(clientId, message);
            }
        }

        public void SecureBroadcastMessage(NetworkMessage<TEnum> message, Guid excludePlayerId)
        {
            foreach (var clientId in _connectedClients.Keys)
            {
                if (clientId.Equals(excludePlayerId))
                    continue;

                SecureSendMessageToPlayer(clientId, message);
            }
        }

        #endregion

        #region Single message

        public void SecureSendMessageToSpecificPlayer(Guid playerId, NetworkMessage<TEnum> message)
        {
            if (_connectedClients.ContainsKey(playerId))
            {
                SecureSendMessageToPlayer(playerId, message);
            }
        }

        public void SendMessageToSpecificPlayer(Guid playerId, NetworkMessage<TEnum> message)
        {
            if (!_serverSettings.UseUdp)
                throw new System.Exception("Server not initialized to use udp");

            if (_connectedClients.ContainsKey(playerId))
            {
                SendMessageToPlayer(playerId, message);
            }
        }

        #endregion

        private void SecureSendMessageToPlayer(Guid playerId, NetworkMessage<TEnum> message)
        {
            _connectedClients[playerId].SecureSendMessage(message);
        }

        private void SendMessageToPlayer(Guid playerId, NetworkMessage<TEnum> message)
        {
            _connectedClients[playerId].SendMessage(message);
        }

    }
}
