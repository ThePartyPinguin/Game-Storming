using System;
using GameFrame.Networking.Messaging.Message;

namespace GameFrame.Networking.Messaging.MessageHandling
{
    public sealed class NetworkMessageCallbackWrapper<TEnum> where TEnum : Enum
    {
        public Type MessageType { get; }
        public Action<NetworkMessage<TEnum>> Callback { get; }

        public NetworkMessageCallbackWrapper(Type messageType, Action<NetworkMessage<TEnum>> callback)
        {
            MessageType = messageType;
            Callback = callback;
        }
    }
}
