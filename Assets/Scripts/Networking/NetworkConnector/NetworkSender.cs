using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.Serialization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GameFrame.Networking.NetworkConnector
{
    abstract class NetworkSender<TEnum> where TEnum : Enum
    {
        private INetworkMessageSerializer<TEnum> _networkMessageSerializer;
        private readonly Queue<NetworkMessage<TEnum>> _networkMessagesQueueToSend;

        private Task _senderTask;
        private bool _senderTaskRunning;
        private ManualResetEvent _waitUntilStopped;
        private bool _setupComplete;

        protected NetworkSender(INetworkMessageSerializer<TEnum> networkMessageSerializer)
        {
            _networkMessageSerializer = networkMessageSerializer;
            _networkMessagesQueueToSend = new Queue<NetworkMessage<TEnum>>();
            _waitUntilStopped = new ManualResetEvent(false);
        }

        public void QueueNewMessageToSend(NetworkMessage<TEnum> message)
        {
            _networkMessagesQueueToSend.Enqueue(message);

            if (!_senderTaskRunning)
            {
                _waitUntilStopped.Reset();
                _senderTaskRunning = true;
                _senderTask = new Task(Send);
                _senderTask.GetAwaiter().OnCompleted(SendTaskStopped);
                _senderTask.Start();
            }
        }

        public void Stop()
        {
            _waitUntilStopped.WaitOne(500);
        }

        private void Send()
        {
            if(!_setupComplete)
                Setup();

            _waitUntilStopped.Set();
            while (_networkMessagesQueueToSend.Count > 0)
            {
                NetworkMessage<TEnum> message = _networkMessagesQueueToSend.Dequeue();
                byte[] data = _networkMessageSerializer.Serialize(message);
                SendMessage(data);
            }
            _senderTaskRunning = false;
        }

        private void SendTaskStopped()
        {
            _waitUntilStopped.Set();
        }

        protected abstract void SendMessage(byte[] data);

        protected virtual void Setup()
        {
            _setupComplete = true;
        }
    }
}
