using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace GameFrame.Networking.Server
{
    public class ServerListener<TEnum> where TEnum : Enum
    {
        private TcpListener _tcpListener;
        private Thread _listenerThread;
        private bool _listenerRunning;

        private ManualResetEvent _waitUntilListenerStopped;
        private Action<TcpClient> _onClientConnect;
        public ServerListener(int port)
        {
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
                _listenerRunning = true;
                //_listenerThread = new Thread(Listen);
                //_listenerThread.GetAwaiter().OnCompleted(OnListenerTaskStopped);
                _listenerThread.Start();
                _waitUntilListenerStopped.Reset();
            }
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
                    Console.WriteLine("Listnening for new connection");
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
