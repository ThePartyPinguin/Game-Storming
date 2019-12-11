using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.UnityHelpers.Networking.UnityMessageEventDatabase.BaseMessage;
using UnityEngine;

public class EventOnlyNetworkMessageCallbackDatabase : UnityBaseMessageEventsDatabase<EventOnlyNetworkMessage, EventOnlyNetworkMessageCallbackDatabase.EventOnlyMessageCallbackWrapper, EventOnlyNetworkMessageCallbackDatabase.EventOnlyMessageCallback>
{
    [Serializable]
    public class EventOnlyMessageCallbackWrapper : BaseMessageCallbackWrapper<EventOnlyNetworkMessage, EventOnlyMessageCallback>
    {
        
    }

    [Serializable]
    public class EventOnlyMessageCallback : BaseMessageCallback<EventOnlyNetworkMessage>
    {
        
    }
}
