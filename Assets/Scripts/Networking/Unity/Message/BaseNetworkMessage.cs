using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using UnityEngine;

public class BaseNetworkMessage : NetworkMessage<NetworkEvent>
{
    public BaseNetworkMessage(NetworkEvent messageEventType) : base(messageEventType)
    {
    }
}
