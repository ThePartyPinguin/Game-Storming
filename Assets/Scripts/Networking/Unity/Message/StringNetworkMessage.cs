using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using UnityEngine;

public class StringNetworkMessage : NetworkMessage<NetworkEvent>
{
    public StringNetworkMessage(NetworkEvent messageEventType) : base(messageEventType)
    {
    }
}
