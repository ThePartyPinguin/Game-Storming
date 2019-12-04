using System;
using System.Net;
using System.Net.Sockets;

namespace GameFrame.Networking.NetworkConnector.Udp
{
    sealed class UdpNetworkSender<TEnum> : NetworkSender<TEnum> where TEnum : Enum
    {
        private UdpClient _udpClient;
        private IPEndPoint _sendEndPoint;

        public UdpNetworkSender(INetworkMessageSerializer<TEnum> networkMessageSerializer, UdpClient udpClient, IPEndPoint sendEndPoint) : base(networkMessageSerializer)
        {
            _udpClient = udpClient;
            _sendEndPoint = sendEndPoint;
        }

        protected override void SendMessage(byte[] data)
        {
            if (data == null || data.Length <= 0)
                return;

            try
            {
                Console.WriteLine("Sending udp message");
                _udpClient.Send(data, data.Length, _sendEndPoint);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
