using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringNetworkMessage : BaseNetworkMessage
{
    public string Value => _value;

    private string _value;
    public StringNetworkMessage(MessageEventType messageEventType, string value) : base(messageEventType)
    {
        _value = value;
    }
}
