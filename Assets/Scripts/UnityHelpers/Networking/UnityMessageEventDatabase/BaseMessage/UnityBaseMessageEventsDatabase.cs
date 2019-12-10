using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.UnityHelpers.Networking.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFrame.UnityHelpers.Networking.UnityMessageEventDatabase.BaseMessage
{
    public abstract class UnityBaseMessageEventsDatabase<TBaseMessage, TBaseCallbackWrapper, TBaseCallback> :  MonoBehaviour
        where TBaseMessage : BaseNetworkMessage
        where TBaseCallbackWrapper : BaseMessageCallbackWrapper<TBaseMessage, TBaseCallback>
        where TBaseCallback : BaseMessageCallback<TBaseMessage>
    {

        [HideInInspector]
        public List<TBaseCallbackWrapper> MessageCallbackWrappers => _messageCallbackWrappers;

        [SerializeField]
        public List<TBaseCallbackWrapper> _messageCallbackWrappers;

        private ASyncToSynchronousMessageHandler _aSyncToSynchronousMessageHandler;
        
        //private Queue<KeyValuePair<TBaseMessage, Guid>> _messagesToHandle;
        //private bool _coRoutineRunning;
        void Awake()
        {
            _aSyncToSynchronousMessageHandler = ASyncToSynchronousMessageHandler.Instance;
            
            RegisterMessageCallbacks();

            MessageCallbackWrappers.Clear();
        }

        private void RegisterMessageCallbacks()
        {
            var callbackDatabase = NetworkEventCallbackDatabase<NetworkEvent>.Instance;

            foreach (var callbackWrapper in MessageCallbackWrappers)
            {
                if (callbackDatabase.CallbackExists(callbackWrapper.EventType))
                {
                    Debug.LogError("Event: " + callbackWrapper.EventType + " already can't be used twice for multiple events, was registered in: " + this.GetType());
                    continue;
                }
                callbackDatabase.RegisterCallBack<TBaseMessage>(callbackWrapper.EventType, AddMessageToQueue);
            }
        }

        public void AddMessageToQueue(TBaseMessage message, Guid clientId)
        {
            _aSyncToSynchronousMessageHandler.QueueMessageToHandle(message, clientId);
        }

        //void Update()
        //{
        //    if (!_coRoutineRunning && _messagesToHandle.Count > 0)
        //    {
        //        _coRoutineRunning = true;
        //        StartCoroutine(HandleMessages());
        //    }
        //}

        //private TBaseCallback GetMessageCallback(NetworkEvent networkEvent)
        //{
        //    return _messageCallbackCollection[networkEvent];
        //}

        //private IEnumerator HandleMessages()
        //{
        //    if (_messageCallbackCollection == null)
        //    {
        //        while (_messageCallbackCollection == null)
        //        {
        //            yield return new WaitForEndOfFrame();
        //        }
        //    }
        //    while (_messagesToHandle.Count > 0)
        //    {
        //        if (_messagesToHandle.Count > 15)
        //        {
        //            HandleAmountOfMessages(15);
        //            yield return new WaitForEndOfFrame();
        //        }
        //        else
        //        {
        //            HandleAmountOfMessages(_messagesToHandle.Count);
        //        }
        //    }

        //    _coRoutineRunning = false;
        //}

        //private void HandleAmountOfMessages(int amount)
        //{
        //    for (int i = 0; i < amount; i++)
        //    {
        //        Debug.Log("Amount to handle: " + amount);

        //        var messagePair = _messagesToHandle.Dequeue();

        //        CallMessageCallback(messagePair.Key, messagePair.Value);
        //    }
        //}

        //private void CallMessageCallback(TBaseMessage message, Guid clientId)
        //{
        
        //    TBaseCallback callback = GetMessageCallback(message.MessageEventType);

        //    callback.Invoke(message, clientId);
        //}
    }
}
