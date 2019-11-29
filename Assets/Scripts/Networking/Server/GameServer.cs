using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.NetworkConnector;
using GameFrame.Networking.Serialization;
using UnityEngine;

namespace GameFrame.Networking.Server
{
    public abstract class GameServer<TEnum> where TEnum : Enum
    {
        private readonly int _port;
        private ServerListener<TEnum> _serverListener;

        protected GameServer(int port)
        {
            _port = port;
        }

        protected void Start(Action<TcpClient> onClientConnect)
        {
            if (_serverListener == null)
                _serverListener = new ServerListener<TEnum>(_port);

            _serverListener.StartListener(onClientConnect);
            Debug.Log("Server started listening for new connections");
        }

        public void Stop()
        {
            _serverListener.StopListener();
        }

        
    }
}
