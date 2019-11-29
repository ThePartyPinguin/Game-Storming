using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using UnityEngine;

public class HandshakeServerResponseMessage : IntMessage
{
    public int ClientId => Value;
    public HandshakeServerResponseMessage(NetworkEvent messageEventType, int clientId) : base(messageEventType, clientId)
    {
    }
}
