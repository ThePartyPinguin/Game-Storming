using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using UnityEngine;

public class StringNetworkMessage : BaseNetworkMessage
{
    public string Value { get; }
    public StringNetworkMessage(NetworkEvent messageEventType, string value) : base(messageEventType)
    {
        Value = value;
    }
}
