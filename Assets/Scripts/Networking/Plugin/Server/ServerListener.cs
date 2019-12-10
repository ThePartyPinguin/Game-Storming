using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GameFrame.Networking.Server
{
    public class ServerListener<TEnum> where TEnum : Enum
    {
        public IPAddress LocalIpAddress => _localIpAddress;
        public int ListenPort => _listenPort;

        private IPAddress _localIpAddress;
        private int _listenPort;

        private TcpListener _tcpListener;
        private Thread _listenerThread;
        private bool _listenerRunning;

        private ManualResetEvent _waitUntilListenerStopped;
        private Action<TcpClient> _onClientConnect;
        public ServerListener(int port)
        {
            _listenPort = port;
            _tcpListener = new TcpListener(IPAddress.Any, port);
            _waitUntilListenerStopped = new ManualResetEvent(false);
            _listenerThread = new Thread(Listen);
        }

        public void StartListener(Action<TcpClient> onClientConnect)
        {
            _onClientConnect = onClientConnect;
            _tcpListener.Start();

            if (!_listenerRunning)
            {
                _localIpAddress = GetLocalIPAddress();

                _listenerRunning = true;
                _listenerThread.Start();
                _waitUntilListenerStopped.Reset();
            }
        }

        public IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new System.Exception("No network adapters with an IPv4 address in the system!");
        }

        public void StopListener()
        {
            _tcpListener.Stop();
            _listenerRunning = false;
            _waitUntilListenerStopped.WaitOne();
        }

        public void Listen()
        {
            _waitUntilListenerStopped.Reset();
            while (_listenerRunning)
            {
                try
                {
                    Console.WriteLine("Listnening for new connection on: " + _localIpAddress + ':' + _listenPort);
                    TcpClient client = _tcpListener.AcceptTcpClient();
                    Console.WriteLine("New connection received");
                    _onClientConnect?.Invoke(client);
                }
                catch(SocketException e)
                {
                    Console.WriteLine(e);
                }
            }

            OnListenerTaskStopped();
        }

        private void OnListenerTaskStopped()
        {
            _waitUntilListenerStopped.Set();
        }
    }
}
