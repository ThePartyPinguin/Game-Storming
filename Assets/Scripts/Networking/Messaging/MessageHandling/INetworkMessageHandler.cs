using System;
using System.Collections.Generic;
using System.Text;
using GameFrame.Networking.Messaging.Message;

namespace GameFrame.Networking.Messaging.MessageHandling
{
    public interface INetworkMessageHandler<in TMessage, TEnum> where TMessage : NetworkMessage<TEnum> where TEnum : Enum
    {
        void MessageHandled(TMessage message);
    }
}
