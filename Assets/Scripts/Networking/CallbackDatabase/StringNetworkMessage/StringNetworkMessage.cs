using System.Collections;
using System.Collections.Generic;
using GameFrame.UnityHelpers.Networking;
using GameFrame.UnityHelpers.Networking.Message;
using UnityEngine;

public class StringNetworkMessage : BaseNetworkMessage
{
    public string Value { get; }

    public StringNetworkMessage(NetworkEvent messageEventType, string value) : base(messageEventType)
    {
        Value = value;
    }
}
