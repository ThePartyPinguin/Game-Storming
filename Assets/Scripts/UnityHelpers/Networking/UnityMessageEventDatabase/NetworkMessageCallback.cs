using GameFrame.Networking.Messaging.Message;
using System;
using UnityEngine.Events;

namespace GameFrame.UnityHelpers.Networking.UnityMessageEventDatabase
{
    [Serializable]
    public class NetworkMessageCallback<TMessage> : UnityEvent<TMessage> where TMessage : NetworkMessage<NetworkEvent>
    {
    
    }
}
