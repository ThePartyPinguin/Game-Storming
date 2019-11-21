using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityBaseMessageNetworkMessageEventsDatabase : UnityNetworkMessageEventsDatabase<BaseNetworkMessage, UnityBaseMessageNetworkMessageEventsDatabase.BaseCallbackWrapper, UnityBaseMessageNetworkMessageEventsDatabase.BaseMessageCallback>
{
    public override List<BaseCallbackWrapper> BaseMessageCallbacks { get; }

    [SerializeField]
    private List<BaseCallbackWrapper> _callbackWrappers;

    [Serializable]
    public class BaseCallbackWrapper : CallbackWrapper<BaseNetworkMessage, BaseMessageCallback>
    {
        public override NetworkEvent EventType => _eventType;

        [SerializeField]
        private NetworkEvent _eventType;

        public override BaseMessageCallback Callback => _callback;

        [SerializeField]
        private BaseMessageCallback _callback;
    }

    [Serializable]
    public class BaseMessageCallback : NetworkMessageCallback<BaseNetworkMessage>
    {

    }
}
