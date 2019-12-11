using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.UnityHelpers.Networking.UnityMessageEventDatabase.BaseMessage;
using UnityEngine;

public class StringNetworkMessageCallbackDatabase : UnityBaseMessageEventsDatabase<StringNetworkMessage, StringNetworkMessageCallbackDatabase.StringNetworkMessageCallbackWrapper, StringNetworkMessageCallbackDatabase.StringNetworkMessageCallback>
{
    [Serializable]
    public class StringNetworkMessageCallbackWrapper : BaseMessageCallbackWrapper<StringNetworkMessage, StringNetworkMessageCallback>
    {
        
    }

    [Serializable]
    public class StringNetworkMessageCallback : BaseMessageCallback<StringNetworkMessage>
    {
        
    }
}
