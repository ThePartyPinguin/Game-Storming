using System;
using System.Collections.Generic;
using System.Text;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.NetworkConnector;
using GameFrame.Networking.Serialization;

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

        protected void Start(HandshakeHandler<TEnum> handshakeHandler)
        {
            if (_serverListener == null)
                _serverListener = new ServerListener<TEnum>(_port, handshakeHandler);

            _serverListener.StartListener();
            Console.WriteLine("Server started listening for new connections");
        }

        public void Stop()
        {
            _serverListener.StopListener();
        }

        
    }
}
