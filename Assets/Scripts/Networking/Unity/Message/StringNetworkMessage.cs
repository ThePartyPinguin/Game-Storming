using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using UnityEngine;

public class StringNetworkMessage : BaseNetworkMessage
{
    public StringNetworkMessage(NetworkEvent messageEventType) : base(messageEventType)
    {
    }
}
