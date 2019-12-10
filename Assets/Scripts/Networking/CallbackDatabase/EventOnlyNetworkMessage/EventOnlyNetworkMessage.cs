using System.Collections;
using System.Collections.Generic;
using GameFrame.UnityHelpers.Networking;
using GameFrame.UnityHelpers.Networking.Message;
using UnityEngine;

public class EventOnlyNetworkMessage : BaseNetworkMessage
{
    public EventOnlyNetworkMessage(NetworkEvent messageEventType) : base(messageEventType)
    {
    }
}
