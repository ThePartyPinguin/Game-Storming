using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkMessage
{
    public MessageType messageType => _messageType;

    private readonly MessageType _messageType;

    protected NetworkMessage(MessageType messageType)
    {
        _messageType = messageType;
    }
}
