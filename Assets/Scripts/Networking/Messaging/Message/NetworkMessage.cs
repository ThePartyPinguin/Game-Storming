using System;

namespace GameFrame.Networking.Messaging.Message
{
    public abstract class NetworkMessage<TEnum> where TEnum : Enum
    {
        public TEnum MessageEventType => _messageEventType;

        private readonly TEnum _messageEventType;

        protected NetworkMessage(TEnum messageEventType)
        {
            _messageEventType = messageEventType;
        }
    }
}
