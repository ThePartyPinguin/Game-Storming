using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkMessageHandler
{
    private readonly Dictionary<MessageType, Action<NetworkMessage>> _messageCallbacks;

    private readonly Queue<NetworkMessage> _messagesToHandleQueue;
    public NetworkMessageHandler()
    {
        _messageCallbacks = new Dictionary<MessageType, Action<NetworkMessage>>();
        _messagesToHandleQueue = new Queue<NetworkMessage>();
    }

    public void AddMessageToQueue(NetworkMessage message)
    {
        _messagesToHandleQueue.Enqueue(message);
    }

    public bool QueueContainsMessages()
    {
        return _messagesToHandleQueue.Count > 0;
    }

    public void RegisterCallBack(MessageType messageType, Action<NetworkMessage> callback)
    {
        if(_messageCallbacks.ContainsKey(messageType))
            throw new MessageTypeAlreadyRegisteredException("Messagetype: " + messageType + " has already been registered in MessageHandler: " + this.GetType());

        _messageCallbacks.Add(messageType, callback);
    }

    public void UnRegisterCallback(MessageType messageType)
    {
        if(!_messageCallbacks.ContainsKey(messageType))
            throw new MessageTypeNotRegisteredException("Messagetype: " + messageType + " has not been registered in MessageHandler: " + this.GetType());

        _messageCallbacks.Remove(messageType);
    }
    public void UnRegisterAllCallbacks()
    {
        _messageCallbacks.Clear();
    }

    public Action<NetworkMessage> GetCallback(MessageType messageType)
    {
        if (!_messageCallbacks.ContainsKey(messageType))
            throw new MessageTypeNotRegisteredException("Messagetype: " + messageType + " has not been registered in MessageHandler: " + this.GetType() + ". Please register this MessageType with: RegisterCallback(MessageType, INetworkMessageCallback<T>)");

        return _messageCallbacks[messageType];
    }

    public void CallCallback(NetworkMessage message)
    {
        if (!_messageCallbacks.ContainsKey(message.messageType))
            throw new MessageTypeNotRegisteredException("Messagetype: " + message.messageType + " has not been registered in MessageHandler: " + this.GetType() + ". Please register this MessageType with: RegisterCallback(MessageType, INetworkMessageCallback<T>)");

        _messageCallbacks[message.messageType].Invoke(message);
    }

    public bool CallbackExists(MessageType messageType)
    {
        return _messageCallbacks.ContainsKey(messageType);
    }
}
