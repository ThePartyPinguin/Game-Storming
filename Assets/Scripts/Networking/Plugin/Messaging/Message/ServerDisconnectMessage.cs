using System;

namespace GameFrame.Networking.Messaging.Message
{
    class ServerDisconnectMessage<TEnum> : NetworkMessage<TEnum> where TEnum : Enum
    {
        public ServerDisconnectMessage(TEnum messageEventType) : base(messageEventType)
        {
        }
    }
}
