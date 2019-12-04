using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using GameFrame.Networking.Messaging;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.Networking.NetworkConnector;
using GameFrame.Networking.Serialization;

namespace GameFrame.Networking.Server
{
    public class GameServer<TEnum> where TEnum : Enum
    {
        private readonly int _tcpPort;
        private readonly int _udpPort;

        private readonly Dictionary<Guid, NetworkConnector<TEnum>> _connectedClients;
        private readonly Dictionary<Guid, NetworkConnector<TEnum>> _connectorsToAccept;

        private ServerListener<TEnum> _serverListener;

        private readonly SerializationType _serializationType;

        private readonly ServerSettings<TEnum> _serverSettings;

        private Action<Guid> _onClientAccepted;

        public GameServer(ServerSettings<TEnum> serverSettings, Action<Guid> onClientAccepted)
        {
            _serverSettings = serverSettings;
            _onClientAccepted = onClientAccepted;

            _connectedClients = new Dictionary<Guid, NetworkConnector<TEnum>>();
            _connectorsToAccept = new Dictionary<Guid, NetworkConnector<TEnum>>();
        }

        public void StartServer()
        {
            if (_serverListener == null)
            {
                _serverListener = new ServerListener<TEnum>(_serverSettings.TcpPort);
                NetworkEventCallbackDatabase<TEnum>.Instance.RegisterCallBack<ClientToServerHandshakeRequest<TEnum>>(_serverSettings.ClientToServerHandshakeEvent, OnReceiveHandshakeRequest);
                NetworkEventCallbackDatabase<TEnum>.Instance.RegisterCallBack<ClientDisconnectMessage<TEnum>>(_serverSettings.ClientDisconnectEvent, OnClientDisconnect);
            }

            _serverListener.StartListener(OnClientConnect);
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
                NetworkConnector<TEnum>.UdpClient.Close();
                NetworkConnector<TEnum>.UdpClient.Dispose();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                Thread.Sleep(10000);
                throw;
            }
        }
        private void RemoveConnection(Guid connectorId)
        {
            if (!_connectedClients.ContainsKey(connectorId))
                return;

            var connector = _connectedClients[connectorId];

            Console.WriteLine("Client disconnect event received");
            connector.Stop();
            _connectedClients.Remove(connectorId);
        }

        #region Callbacks

        private void OnClientConnect(TcpClient client)
        {
            Guid guid = Guid.NewGuid();

            var connector = new NetworkConnector<TEnum>(guid, client, _serverSettings.UdpRemoteSendPort, _serverSettings.UdpReceivePort);
            connector.Setup(_serializationType);
            connector.SetupCallbacks(() => { }, () => OnConnectionLost(guid));
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
                _connectedClients.Add(connectorId, connector);

                if(_serverSettings.UseUdp)
                    connector.StartUdp();

                connector.SecureSendMessage(new ServerToClientHandshakeResponse<TEnum>(_serverSettings.ServerToClientHandshakeEvent, true, connectorId));
                _onClientAccepted?.Invoke(connectorId);
            }
        }

        private void OnClientDisconnect(ClientDisconnectMessage<TEnum> message, Guid connectorId)
        {
            if (!_connectedClients.ContainsKey(connectorId) && message.MessageEventType.Equals(_serverSettings.ClientDisconnectEvent))
                return;

            Console.WriteLine("Client disconnect event received");
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
