using GameFrame.Networking.Messaging.MessageHandling;
using System;
using System.Net.Sockets;
using System.Threading;

namespace GameFrame.Networking.NetworkConnector.Tcp
{
    class TcpNetworkReceiver<TEnum> : NetworkReceiver<TEnum> where TEnum : Enum
    {
        private readonly TcpClient _tcpClient;

        private NetworkStream _networkStream;

        private Action _onConnectionLost;
        public TcpNetworkReceiver(NetworkMessageDeserializer<TEnum> messageDeserializer, TcpClient tcpClient, Action onConnectionLost) : base(messageDeserializer)
        {
            _tcpClient = tcpClient;
            _onConnectionLost = onConnectionLost;
        }

        protected override void Setup()
        {
            _networkStream = _tcpClient.GetStream();
            base.Setup();
        }

        protected override void Stop()
        {
            _networkStream.Close();
            _networkStream.Dispose();

            _tcpClient.Close();
            _tcpClient.Dispose();
        }

        protected override byte[] ReceiveData()
        {
            if (_networkStream == null)
                _networkStream = _tcpClient.GetStream();
            try
            {
                int dataAvailable = _tcpClient.Available;
                if (dataAvailable > 0)
                { 
                    byte[] buffer = new byte[dataAvailable];
                    _networkStream.Read(buffer, 0, buffer.Length);
                    return buffer;
                }
                else
                {
                    Thread.Sleep(100);
                    return null;
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                _onConnectionLost?.Invoke();
                return null;
            }
        }
    }
}
