using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityIdeaMessageEventDatabase : UnityBaseMessageEventsDatabase<IdeaNetworkMessage, UnityIdeaMessageEventDatabase.IdeaMessageCallbackWrapper, UnityIdeaMessageEventDatabase.IdeaMessageCallback>
{
    [Serializable]
    public class IdeaMessageCallbackWrapper : BaseMessageCallbackWrapper<IdeaNetworkMessage, IdeaMessageCallback>
    {
    }

    [Serializable]
    public class IdeaMessageCallback : BaseMessageCallback<IdeaNetworkMessage>
    {

    }
}
