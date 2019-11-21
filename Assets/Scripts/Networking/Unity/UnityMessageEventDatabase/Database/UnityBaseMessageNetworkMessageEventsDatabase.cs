using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class UnityBaseMessageEventsDatabase<TBaseMessage, TBaseCallbackWrapper, TBaseCallback> :  MonoBehaviour
    where TBaseMessage : BaseNetworkMessage
    where TBaseCallbackWrapper : BaseMessageCallbackWrapper<TBaseMessage, TBaseCallback>
    where TBaseCallback : BaseMessageCallback<TBaseMessage>
{

    [HideInInspector]
    public List<TBaseCallbackWrapper> MessageCallbackWrappers => _messageCallbackWrappers;

    [SerializeField]
    public List<TBaseCallbackWrapper> _messageCallbackWrappers;

    private Dictionary<NetworkEvent, TBaseCallback> _messageCallbackCollection;

    void Start()
    {
        _messageCallbackCollection = new Dictionary<NetworkEvent, TBaseCallback>();
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

    public Type GetDatabaseMessageType()
    {
        return typeof(TBaseMessage);
    }

    public TBaseCallback GetMessageCallback(NetworkEvent networkEvent)
    {
        return _messageCallbackCollection[networkEvent];
    }
}
