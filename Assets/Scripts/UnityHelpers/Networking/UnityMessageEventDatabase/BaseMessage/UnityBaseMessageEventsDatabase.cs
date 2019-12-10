using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.UnityHelpers.Networking.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Server;
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

        private ASyncToSynchronousCallbackHandler _aSyncToSynchronousMessageHandler;
        
        void Awake()
        {
            _aSyncToSynchronousMessageHandler = ASyncToSynchronousCallbackHandler.Instance;
            
            RegisterMessageCallbacks();
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
                callbackDatabase.RegisterCallBack<TBaseMessage>(callbackWrapper.EventType, (message, clientId) =>
                {
                    _aSyncToSynchronousMessageHandler.QueueCallbackToHandle(() =>
                    {
                        callbackWrapper.Callback.Invoke(message, clientId);
                    });
                });
            }
        }
    }
}
