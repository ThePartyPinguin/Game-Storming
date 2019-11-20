using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using UnityEngine;

[Serializable]
public class BaseNetworkMessage : NetworkMessage<MessageEventType>
{
    public BaseNetworkMessage(MessageEventType messageEventType) : base(messageEventType)
    {
    }
}
