using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class NetworkReceiver
{
    protected NetworkMessageHandler _messageHandler;

    private ISerializer _serializer;

    private Task _receiverTask;

    private ManualResetEvent _waitUntilTaskFinished;

    private bool _running;
    protected NetworkReceiver(ISerializer serializer)
    {
        _messageHandler = new NetworkMessageHandler();
        _serializer = serializer;
        _receiverTask = new Task(Receive);
        _receiverTask.GetAwaiter().OnCompleted(ReleaseWaitUntilFinished);
        _waitUntilTaskFinished = new ManualResetEvent(true);
    }

    private void ReleaseWaitUntilFinished()
    {
        _waitUntilTaskFinished.Reset();
    }

    public void StopReceiving()
    {
        _running = false;
        _waitUntilTaskFinished.WaitOne();
    }
    
    private async void Receive()
    {
        while (_running)
        {
            byte[] data = ReceiveData();

            if (data != null && data.Length > 0)
            {
                HandleData(data);
            }
            else
            {
                await Task.Delay(100);
            }
        }

        _waitUntilTaskFinished.Reset();
    }

    protected abstract byte[] ReceiveData();

    public void RegisterNewMessageHandler(NetworkMessageHandler messageHandler)
    {
        _messageHandler = messageHandler;
    }

    private NetworkMessage DeSerializeMessage(byte[] data)
    {
        return _serializer.DeSerialize(data);
    }

    protected void HandleData(byte[] data)
    {
        NetworkMessage message = DeSerializeMessage(data);
        _messageHandler.AddMessageToQueue(message);
    }

}
