using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.NetworkConnector;
using UnityEngine;

namespace GameFrame.Networking.Server
{
    public class ServerListener<TEnum> where TEnum : Enum
    {
        private TcpListener _tcpListener;
        private Task _listenerTask;
        private bool _listenerRunning;

        private ManualResetEvent _waitUntilListenerStopped;
        private Action<TcpClient> _onClientConnect;
        public ServerListener(int port)
        {
            _tcpListener = new TcpListener(IPAddress.Any, port);
            _waitUntilListenerStopped = new ManualResetEvent(false);
        }

        public void StartListener(Action<TcpClient> onClientConnect)
        {
            _onClientConnect = onClientConnect;
            _tcpListener.Start();

            if (!_listenerRunning)
            {
                _listenerRunning = true;
                _listenerTask = new Task(Listen);
                _listenerTask.GetAwaiter().OnCompleted(OnListenerTaskStopped);
                _listenerTask.Start();
            }
        }

        public void StopListener()
        {
            _tcpListener.Stop();
            _listenerRunning = false;
            _waitUntilListenerStopped.WaitOne(500);
        }

        public void Listen()
        {
            _waitUntilListenerStopped.Reset();
            while (_listenerRunning)
            {
                try
                {
                    TcpClient client = _tcpListener.AcceptTcpClient();
                    Debug.Log("New connection received");
                    _onClientConnect?.Invoke(client);
                }
                catch(SocketException e)
                {

                }
            }
        }

        private void OnListenerTaskStopped()
        {
            _waitUntilListenerStopped.Set();
        }
    }
}
