using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using UnityEngine;
using UnityEngine.Events;

public abstract class UnityNetworkMessageEventsDatabase<TMessage, TCallbackWrapper, TCallBack> : MonoBehaviour, INetworkMessageEventsDatabase<TMessage>
    where TMessage : NetworkMessage<NetworkEvent>
    where TCallBack : UnityEvent<TMessage>
    where TCallbackWrapper : NetworkMessageCallbackWrapper<TMessage, TCallBack>
{
    public abstract List<TCallbackWrapper> MessageCallbackWrappers { get; }
    //public List<TCallbackWrapper> MessageCallbackWrappers => MessageCallbackWrappers;

    //[SerializeField]
    //private List<TCallbackWrapper> MessageCallbackWrappers;

    private Dictionary<NetworkEvent, UnityEvent<TMessage>> _messageCallbackCollection;
    void Start()
    {
        foreach (var callbackWrapper in MessageCallbackWrappers)
        {
            if (_messageCallbackCollection.ContainsKey(callbackWrapper.EventType))
            {
                Debug.LogError("Event: " + callbackWrapper.EventType + " already can't be used twice for multiple events");
                continue;
            }
            _messageCallbackCollection.Add(callbackWrapper.EventType, callbackWrapper.Callback);
        }

        MessageCallbackWrappers.Clear();
    }

    public void Call(TMessage message)
    {
        if (!_messageCallbackCollection.ContainsKey(message.MessageEventType))
        {

        }
    }

    public Type GetDatabaseMessageType()
    {
        return typeof(TMessage);
    }

    public UnityEvent<TMessage> GetMessageCallback(NetworkEvent networkEvent)
    {
        throw new NotImplementedException();
    }
}
