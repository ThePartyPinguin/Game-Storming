using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOnlyNetworkMessage : BaseNetworkMessage
{
    public EventOnlyNetworkMessage(NetworkEvent messageEventType) : base(messageEventType)
    {
    }
}
