using GameFrame.Networking.Messaging.MessageHandling;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GameFrame.Networking.NetworkConnector.Udp
{
    sealed class UdpNetworkReceiver<TEnum> : NetworkReceiver<TEnum> where TEnum : Enum
    {
        private UdpClient _udpClient;
        private IPEndPoint _receiveEndPoint;
        public UdpNetworkReceiver(NetworkMessageDeserializer<TEnum> messageDeserializer, UdpClient udpClient, IPEndPoint receiveEndPoint) : base(messageDeserializer)
        {
            _udpClient = udpClient;
            _receiveEndPoint = receiveEndPoint;
            Console.WriteLine(_receiveEndPoint);
        }

        protected override void Stop()
        {
            
        }

        protected override byte[] ReceiveData()
        {
            try
            {
                int bytesAvailable = _udpClient.Available;

                if (bytesAvailable > 0)
                {
                    return _udpClient.Receive(ref _receiveEndPoint);
                }
                else
                {
                    Thread.Sleep(10);
                    return null;
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
