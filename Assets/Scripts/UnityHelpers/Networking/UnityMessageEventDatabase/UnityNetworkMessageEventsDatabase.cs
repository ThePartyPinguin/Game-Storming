using GameFrame.Networking.Messaging.Message;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameFrame.UnityHelpers.Networking.UnityMessageEventDatabase
{
    public abstract class UnityNetworkMessageEventsDatabase<TMessage, TCallbackWrapper, TCallBack> : MonoBehaviour
        where TMessage : NetworkMessage<NetworkEvent>
        where TCallBack : UnityEvent<TMessage, Guid>
        where TCallbackWrapper : NetworkMessageCallbackWrapper<TMessage, TCallBack>
    {
    
    }
}
