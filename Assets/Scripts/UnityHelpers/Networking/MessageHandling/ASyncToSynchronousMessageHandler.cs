using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.UnityHelpers.Networking;
using GameFrame.UnityHelpers.Networking.Message;
using UnityEngine;

public class ASyncToSynchronousMessageHandler : MonoSingleton<ASyncToSynchronousMessageHandler>
{
    private NetworkEventCallbackDatabase<NetworkEvent> _callbackDatabase;

    private bool _coRoutineRunning;
    private Queue<KeyValuePair<BaseNetworkMessage, Guid>> _messageToHandle;

    void Start()
    {
        _callbackDatabase = NetworkEventCallbackDatabase<NetworkEvent>.Instance;
        _messageToHandle = new Queue<KeyValuePair<BaseNetworkMessage, Guid>>();
    }

    public void QueueMessageToHandle(BaseNetworkMessage message, Guid connectorId)
    {
        _messageToHandle.Enqueue(new KeyValuePair<BaseNetworkMessage, Guid>(message, connectorId));
    }

    void Update()
    {
        if (!_coRoutineRunning && _messageToHandle.Count > 0)
        {
            _coRoutineRunning = true;
            StartCoroutine(HandleCoRoutine());
        }
    }

    private IEnumerator HandleCoRoutine()
    {
        while (_messageToHandle.Count > 0)
        {
            if (_messageToHandle.Count > 15)
            {
                HandleMessages(15);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                HandleMessages(_messageToHandle.Count);
                yield return new WaitForEndOfFrame();
            }
        }

        _coRoutineRunning = false;
    }

    private void HandleMessages(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var messagePair = _messageToHandle.Dequeue();

            var message = messagePair.Key;
            var guid = messagePair.Value;

            var wrapper = _callbackDatabase.GetCallbackWrapper(message.MessageEventType);

            wrapper.Callback.Invoke(message, guid);
        }
        
    }
}
