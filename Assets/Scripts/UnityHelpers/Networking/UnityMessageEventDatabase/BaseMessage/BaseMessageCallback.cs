using System;
using UnityEngine.Events;

namespace GameFrame.UnityHelpers.Networking.UnityMessageEventDatabase.BaseMessage
{
    [Serializable]
    public class BaseMessageCallback<TBaseMessage> : UnityEvent<TBaseMessage, Guid>
    {

    }
}
