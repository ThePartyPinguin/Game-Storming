using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class NetworkMessageCallbackWrapper<TMessage, TCallback> where TMessage : NetworkMessage<NetworkEvent> where TCallback : UnityEvent<TMessage, Guid>
{
    public abstract NetworkEvent EventType { get; }
    public abstract TCallback Callback { get; }
}
