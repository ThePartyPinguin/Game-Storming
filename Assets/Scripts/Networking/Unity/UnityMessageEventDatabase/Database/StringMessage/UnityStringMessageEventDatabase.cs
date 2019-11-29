using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityStringMessageEventDatabase : UnityBaseMessageEventsDatabase<StringNetworkMessage, UnityStringMessageEventDatabase.StringCallbackWrapper, UnityStringMessageEventDatabase.StringMessageCallback>
{
    [Serializable]
    public class StringCallbackWrapper : BaseMessageCallbackWrapper<StringNetworkMessage, StringMessageCallback>
    {
    }

    [Serializable]
    public class StringMessageCallback : BaseMessageCallback<StringNetworkMessage>
    {

    }
}