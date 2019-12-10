using GameFrame.Networking.Messaging.Message;
using System;

namespace GameFrame.Networking.Messaging.MessageHandling
{
    public sealed class NetworkMessageCallbackWrapper<TEnum> where TEnum : Enum
    {
        public Type MessageType { get; }
        public Action<NetworkMessage<TEnum>, Guid> Callback { get; }

        public NetworkMessageCallbackWrapper(Type messageType, Action<NetworkMessage<TEnum>, Guid> callback)
        {
            MessageType = messageType;
            Callback = callback;
        }
    }
}
