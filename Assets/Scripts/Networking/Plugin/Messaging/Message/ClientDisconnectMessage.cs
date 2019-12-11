using System;

namespace GameFrame.Networking.Messaging.Message
{
    class ClientDisconnectMessage<TEnum> : NetworkMessage<TEnum> where TEnum : Enum
    {
        public ClientDisconnectMessage(TEnum messageEventType) : base(messageEventType)
        {
        }
    }
}
