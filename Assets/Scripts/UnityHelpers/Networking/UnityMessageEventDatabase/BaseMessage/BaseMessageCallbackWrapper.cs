using GameFrame.UnityHelpers.Networking.Message;
using System;
using UnityEngine;

namespace GameFrame.UnityHelpers.Networking.UnityMessageEventDatabase.BaseMessage
{
    [Serializable]
    public class BaseMessageCallbackWrapper<TBaseMessage, TBaseCallback> : NetworkMessageCallbackWrapper<TBaseMessage, TBaseCallback> 
        where TBaseMessage : BaseNetworkMessage 
        where TBaseCallback : BaseMessageCallback<TBaseMessage>
    {
        public override NetworkEvent EventType => _eventType;

        [SerializeField]
        private NetworkEvent _eventType;

        public override TBaseCallback Callback => _callback;

        [SerializeField]
        public TBaseCallback _callback;
    }
}
