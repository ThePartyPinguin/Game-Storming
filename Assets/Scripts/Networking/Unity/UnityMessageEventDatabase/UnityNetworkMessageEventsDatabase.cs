using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using UnityEngine;
using UnityEngine.Events;

public abstract class UnityNetworkMessageEventsDatabase<TMessage, TCallbackWrapper, TCallBack> : MonoBehaviour
    where TMessage : NetworkMessage<NetworkEvent>
    where TCallBack : UnityEvent<TMessage, Guid>
    where TCallbackWrapper : NetworkMessageCallbackWrapper<TMessage, TCallBack>
{
    
}
