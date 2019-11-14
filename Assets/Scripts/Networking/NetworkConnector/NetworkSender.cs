using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class NetworkSender
{
    private ISerializer _serializer;
    private readonly Queue<NetworkMessage> _networkMessagesQueueToSend;

    private Task _senderTask;
    private bool _senderTaskRunning;

    protected NetworkSender(ISerializer serializer)
    {
        _serializer = serializer;
        _networkMessagesQueueToSend = new Queue<NetworkMessage>();
        _senderTask = new Task(Send);
    }

    public void QueueNewMessageToSend(NetworkMessage message)
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
            byte[] data = _serializer.Serialize(_networkMessagesQueueToSend.Dequeue());

            SendMessage(data);
        }
        _senderTaskRunning = false;
    }

    protected abstract void SendMessage(byte[] data);
}
