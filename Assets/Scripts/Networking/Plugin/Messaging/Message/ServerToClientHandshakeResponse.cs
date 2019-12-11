using System;

namespace GameFrame.Networking.Messaging.Message
{
    public class ServerToClientHandshakeResponse<TEnum> : NetworkMessage<TEnum> where TEnum : Enum
    {
        public bool Accepted { get; }
        public Guid ClientId { get; }
        public ServerToClientHandshakeResponse(TEnum messageEventType, bool accepted, Guid clientId) : base(messageEventType)
        {
            Accepted = accepted;
            ClientId = clientId;
        }
    }
}
