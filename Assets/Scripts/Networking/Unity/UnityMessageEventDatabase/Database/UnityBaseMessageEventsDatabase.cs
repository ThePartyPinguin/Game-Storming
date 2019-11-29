﻿using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.MessageHandling;
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

    private Queue<TBaseMessage> _messagesToHandle;
    private bool _coRoutineRunning;
    void Start()
    {
        _messagesToHandle = new Queue<TBaseMessage>();
        _messageCallbackCollection = new Dictionary<NetworkEvent, TBaseCallback>();
        foreach (var callbackWrapper in MessageCallbackWrappers)
        {
            if (_messageCallbackCollection.ContainsKey(callbackWrapper.EventType))
            {
                Debug.LogError("Event: " + callbackWrapper.EventType + " already can't be used twice for multiple events");
                continue;
            }
            _messageCallbackCollection.Add(callbackWrapper.EventType, callbackWrapper.Callback);

            NetworkMessageCallbackDatabase<NetworkEvent>.Instance.RegisterCallBack<TBaseMessage>(callbackWrapper.EventType,
                (message, connector) =>
                {
                    AddMessageToQueue(message);
                });
        }

        MessageCallbackWrappers.Clear();
    }

    public void AddMessageToQueue(TBaseMessage message)
    {
        _messagesToHandle.Enqueue(message);
    }

    void Update()
    {
        if (!_coRoutineRunning && _messagesToHandle.Count > 0)
        {
            _coRoutineRunning = true;
            StartCoroutine(HandleMessages());
        }
    }

    private TBaseCallback GetMessageCallback(NetworkEvent networkEvent)
    {
        return _messageCallbackCollection[networkEvent];
    }

    private IEnumerator HandleMessages()
    {
        while (_messagesToHandle.Count > 0)
        {
            if (_messagesToHandle.Count > 15)
            {
                HandleAmountOfMessages(15);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                HandleAmountOfMessages(_messagesToHandle.Count);
            }
        }

        _coRoutineRunning = false;
    }

    private void HandleAmountOfMessages(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            CallMessageCallback(_messagesToHandle.Dequeue());
        }
    }

    private void CallMessageCallback(TBaseMessage message)
    {

        TBaseCallback callback = GetMessageCallback(message.MessageEventType);

        callback.Invoke(message);
    }
}
