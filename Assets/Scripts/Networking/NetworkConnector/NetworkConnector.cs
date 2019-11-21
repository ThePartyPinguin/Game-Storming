using System;
using System.Net;
using System.Net.Sockets;
using GameFrame.Networking.Exception;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.Networking.Serialization;

namespace GameFrame.Networking.NetworkConnector
{
    public class NetworkConnector<TEnum> where TEnum : Enum
    {
        public IPAddress IpAddress { get; }

        private NetworkReceiver<TEnum> _receiver;
        private NetworkSender<TEnum> _sender;

        private IPEndPoint _endPoint;
        private TcpClient _tcpClient;

        private INetworkMessageSerializer<TEnum> _networkMessageSerializer;
        private bool _setupComplete;

        public NetworkConnector(string ipAddress, int port)
        {
            if (IPAddress.TryParse(ipAddress, out IPAddress ip))
            {
                IpAddress = ip;

                _endPoint = new IPEndPoint(IpAddress, port);
            }
            else
            {
                throw new InvalidIPAdressException("The given ipAdress: " + IpAddress + " could not be parsed");
            }
        }

        public NetworkConnector(IPAddress ipAddress, int port)
        {
            IpAddress = ipAddress;
            _endPoint = new IPEndPoint(IpAddress, port);
        }

        /// <summary>
        /// Setup the sender and receiver
        /// </summary>
        /// <param name="initialMessageCallbackDatabase">Initial set of callback that can be handled after connecting</param>
        /// <param name="serializationType">The used serializationType</param>
        public void Setup(Action<NetworkMessage<TEnum>, Type> onMessageReceived, SerializationType serializationType)
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

            _receiver = new TcpNetworkReceiver<TEnum>(_tcpClient);

            _receiver.RegisterNewMessageHandler(new NetworkMessageDeserializer<TEnum>(onMessageReceived, _networkMessageSerializer));

            _sender = new TcpNetworkSender<TEnum>(_networkMessageSerializer, _tcpClient);

            _setupComplete = true;
        }

        private void SetupTcpClient()
        {
            _tcpClient = new TcpClient(_endPoint);
        }

        /// <summary>
        /// Try to connect the tcp client, if succeed, start the message receiver
        /// </summary>
        public void Connect()
        {
            if(!_setupComplete)
                throw new NotSetupCorrectlyException("The " + this.GetType() + " was not setup correct, please run setup() before connecting");

            try
            {
                _tcpClient.Connect(_endPoint);
                _receiver.StartReceiving();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
