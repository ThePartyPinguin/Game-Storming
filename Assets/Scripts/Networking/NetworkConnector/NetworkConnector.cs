using GameFrame.Networking.Exception;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.Networking.NetworkConnector.Tcp;
using GameFrame.Networking.NetworkConnector.Udp;
using GameFrame.Networking.Serialization;
using GameFrame.Networking.Serialization.Json;
using System;
using System.Net;
using System.Net.Sockets;

namespace GameFrame.Networking.NetworkConnector
{
    public class NetworkConnector<TEnum> where TEnum : Enum
    {
        private readonly IPAddress _ipAddress;
        private readonly int _tcpReceivePort;

        private TcpClient _tcpClient;
        private NetworkReceiver<TEnum> _tcpReceiver;
        private NetworkSender<TEnum> _tcpSender;

        private static UdpClient _udpClient;

        private readonly int _udpRemoteSendPort;
        private readonly int _udpReceivePort;
        private NetworkReceiver<TEnum> _udpReceiver;
        private NetworkSender<TEnum> _udpSender;

        private INetworkMessageSerializer<TEnum> _networkMessageSerializer;
        private bool _setupComplete;

        private Action _onConnectionLost;
        private Action _onConnectionFailed;

        public const int SIO_UDP_CONNRESET = -1744830452;

        public Guid Guid { get; private set; }

        public static UdpClient UdpClient => _udpClient;

        public NetworkConnector(IPAddress ipAddress, int tcpReceivePort, int udpRemoteSendPort, int udpReceivePort)
        {
            _ipAddress = ipAddress;
            _tcpReceivePort = tcpReceivePort;
            _udpReceivePort = udpReceivePort;
            _udpRemoteSendPort = udpRemoteSendPort;
        }

        public NetworkConnector(Guid guid, TcpClient tcpClient, int udpRemoteSendPort, int udpReceivePort)
        {
            Guid = guid;
            _tcpClient = tcpClient;
            _udpRemoteSendPort = udpRemoteSendPort;
            _udpReceivePort = udpReceivePort;
        }

        /// <summary>
        /// Setup the sender and receiver
        /// </summary>
        /// <param name="onMessageReceived">this callback get's called when the message deserializer has has deserialized a new message</param>
        /// <param name="serializationType">The used serializationType</param>
        /// <param name="onConnectionLost">This callback get's called when the receiver or sender throws an exception</param>
        public void Setup(SerializationType serializationType)
        {
            if(_setupComplete)
                throw new AlreadySetupException("The: " + this.GetType() + " has already been setup");

            switch (serializationType)
            {
                case SerializationType.JSON:
                    _networkMessageSerializer = new JsonNetworkMessageSerializer<TEnum>();
                    break;
            }
            
            _setupComplete = true;
        }

        public void SetupCallbacks(Action onConnectionFailed, Action onConnectionLost)
        {
            _onConnectionLost = onConnectionLost;
            _onConnectionFailed = onConnectionFailed;
        }

        public void StartTcp()
        {
            _tcpReceiver = new TcpNetworkReceiver<TEnum>(new NetworkMessageDeserializer<TEnum>(this, _networkMessageSerializer), _tcpClient, OnConnectionLost);
            _tcpSender = new TcpNetworkSender<TEnum>(_networkMessageSerializer, _tcpClient, OnConnectionLost);
            _tcpReceiver.StartReceiving();
        }

        public void StartUdp()
        {
            Console.WriteLine("Setting up upd connection");

            if (UdpClient == null)
            {
                _udpClient = new UdpClient(_udpReceivePort);
                UdpClient.DontFragment = true;
                UdpClient.EnableBroadcast = true;

                UdpClient.Client.IOControl(
                    (IOControlCode)SIO_UDP_CONNRESET,
                    new byte[] { 0, 0, 0, 0 },
                    null
                );
            }

            IPAddress remoteIpAddress = ((IPEndPoint) _tcpClient.Client.RemoteEndPoint).Address;

            _udpReceiver = new UdpNetworkReceiver<TEnum>(new NetworkMessageDeserializer<TEnum>(this, _networkMessageSerializer), UdpClient, new IPEndPoint(remoteIpAddress, _udpReceivePort));
            _udpSender = new UdpNetworkSender<TEnum>(_networkMessageSerializer, UdpClient, new IPEndPoint(remoteIpAddress, _udpRemoteSendPort));

            _udpReceiver.StartReceiving();
        }

        private void OnConnectionLost()
        {
            _onConnectionLost?.Invoke();
        }
        /// <summary>
        /// Try to connect the tcp client, if succeed, start the message receiver
        /// </summary>
        public void Connect()
        {
            if (!_setupComplete)
                throw new NotSetupCorrectlyException("The " + this.GetType() + " was not setup correct, please run setup() before connecting");

            try
            {
                _tcpClient = new TcpClient();
                _tcpClient.Connect(_ipAddress, _tcpReceivePort);
                Console.WriteLine("Connected");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                _onConnectionFailed?.Invoke();
                throw;
            }
        }

        public void SecureSendMessage(NetworkMessage<TEnum> message)
        {
            if (!_setupComplete)
                throw new NotSetupCorrectlyException("The: " + this.GetType() + " has not been setup yet");

            _tcpSender.QueueNewMessageToSend(message);
        }

        public void SendMessage(NetworkMessage<TEnum> message)
        {
            if (!_setupComplete)
                throw new NotSetupCorrectlyException("The: " + this.GetType() + " has not been setup yet");

            _udpSender.QueueNewMessageToSend(message);
        }

        public void Stop()
        {
            try
            {
                _tcpSender?.Stop();
                _tcpReceiver?.StopReceiving();

                _udpSender?.Stop();
                _udpReceiver?.StopReceiving();

                Console.WriteLine("Client stopped");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
