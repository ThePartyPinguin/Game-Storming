using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityEventOnlyMessageEventDatabase : UnityBaseMessageEventsDatabase<EventOnlyNetworkMessage, UnityEventOnlyMessageEventDatabase.EventOnlyCallbackWrapper, UnityEventOnlyMessageEventDatabase.EventOnlyMessageCallback>
{
    [Serializable]
    public class EventOnlyCallbackWrapper : BaseMessageCallbackWrapper<EventOnlyNetworkMessage, EventOnlyMessageCallback>
    {
    }

    [Serializable]
    public class EventOnlyMessageCallback : BaseMessageCallback<EventOnlyNetworkMessage>
    {

    }
}
