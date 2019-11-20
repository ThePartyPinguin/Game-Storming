using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using UnityEngine;
using UnityEngine.Events;

public abstract class UnityNetworkMessageEventsDatabase<TMessage, TCallbackWrapper, TCallBack> : MonoBehaviour
    where TMessage : BaseNetworkMessage 
    where TCallbackWrapper : UnityNetworkMessageEventsDatabase<TMessage, TCallbackWrapper, TCallBack>.CallbackWrapper<TMessage, TCallBack>
    where TCallBack : UnityNetworkMessageEventsDatabase<TMessage, TCallbackWrapper, TCallBack>.NetworkMessageCallback<TMessage>
{
    public abstract List<TCallbackWrapper> BaseMessageCallbacks { get; }


    [Serializable]
    public abstract class CallbackWrapper<TMessage, TCallback> where TMessage : BaseNetworkMessage where TCallback : NetworkMessageCallback<TMessage>
    {
        public abstract MessageEventType EventType { get; }
        public abstract TCallback Callback { get; }
    }

    [Serializable]
    public class NetworkMessageCallback<TMessage> : UnityEvent<TMessage> where TMessage : BaseNetworkMessage
    {

    }
}
