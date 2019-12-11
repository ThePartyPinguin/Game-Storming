using System;

namespace GameFrame.Networking.Messaging.Message
{
    public class ClientToServerHandshakeRequest<TEnum> : NetworkMessage<TEnum> where TEnum : Enum
    {
        public ClientToServerHandshakeRequest(TEnum messageEventType) : base(messageEventType)
        {
        }
    }
}
