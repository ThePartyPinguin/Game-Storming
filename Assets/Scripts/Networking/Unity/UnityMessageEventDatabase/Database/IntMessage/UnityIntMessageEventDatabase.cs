using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityIntMessageEventDatabase : UnityNetworkMessageEventsDatabase<IntMessage, UnityIntMessageEventDatabase.IntMessageCallbackWrapper, UnityIntMessageEventDatabase.IntMessageCallback>
{
    [Serializable]
    public class IntMessageCallbackWrapper : BaseMessageCallbackWrapper<IntMessage, IntMessageCallback>
    {
    }

    [Serializable]
    public class IntMessageCallback : BaseMessageCallback<IntMessage>
    {

    }
}

