using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using UnityEngine;

public class IntMessage : BaseNetworkMessage
{
    internal int Value { get; }
    public IntMessage(NetworkEvent messageEventType, int value) : base(messageEventType)
    {
        Value = value;
    }
}
