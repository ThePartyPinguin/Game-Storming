using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFrame.Networking.Messaging.Message;

namespace GameFrame.Networking.NetworkConnector
{
    abstract class NetworkSender<TEnum> where TEnum : Enum
    {
        private INetworkMessageSerializer<TEnum> _networkMessageSerializer;
        private readonly Queue<NetworkMessage<TEnum>> _networkMessagesQueueToSend;

        private Task _senderTask;
        private bool _senderTaskRunning;

        protected NetworkSender(INetworkMessageSerializer<TEnum> networkMessageSerializer)
        {
            _networkMessageSerializer = networkMessageSerializer;
            _networkMessagesQueueToSend = new Queue<NetworkMessage<TEnum>>();
            _senderTask = new Task(Send);
        }

        public void QueueNewMessageToSend(NetworkMessage<TEnum> message)
        {
            _networkMessagesQueueToSend.Enqueue(message);

            if (!_senderTaskRunning)
            {
                _senderTaskRunning = true;
                _senderTask.Start();
            }
        }

        private void Send()
        {
            while (_networkMessagesQueueToSend.Count > 0)
            {
                byte[] data = _networkMessageSerializer.Serialize(_networkMessagesQueueToSend.Dequeue());

                SendMessage(data);
            }
            _senderTaskRunning = false;
        }

        protected abstract void SendMessage(byte[] data);
    }
}
