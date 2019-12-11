using GameFrame.Networking.Messaging.Message;
using System;
using UnityEngine.Events;

namespace GameFrame.UnityHelpers.Networking.UnityMessageEventDatabase
{
    [Serializable]
    public abstract class NetworkMessageCallbackWrapper<TMessage, TCallback> where TMessage : NetworkMessage<NetworkEvent> where TCallback : UnityEvent<TMessage, Guid>
    {
        public abstract NetworkEvent EventType { get; }
        public abstract TCallback Callback { get; }
    }
}
