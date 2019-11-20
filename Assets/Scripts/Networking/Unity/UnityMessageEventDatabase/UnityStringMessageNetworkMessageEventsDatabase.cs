using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityStringMessageNetworkMessageEventsDatabase : UnityNetworkMessageEventsDatabase<StringNetworkMessage, UnityStringMessageNetworkMessageEventsDatabase.StringCallbackWrapper, UnityStringMessageNetworkMessageEventsDatabase.StringMessageCallback>
{
    public override List<StringCallbackWrapper> BaseMessageCallbacks { get; }

    [SerializeField]
    private List<StringCallbackWrapper> _callbackWrappers;

    [Serializable]
    public class StringCallbackWrapper : CallbackWrapper<StringNetworkMessage, StringMessageCallback>
    {
        public override MessageEventType EventType => _eventType;

        [SerializeField]
        private MessageEventType _eventType;

        public override StringMessageCallback Callback => _callback;

        [SerializeField]
        private StringMessageCallback _callback;
    }

    [Serializable]
    public class StringMessageCallback : NetworkMessageCallback<StringNetworkMessage>
    {

    }


}