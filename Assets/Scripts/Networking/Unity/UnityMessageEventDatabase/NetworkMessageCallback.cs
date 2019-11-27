using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class NetworkMessageCallback<TMessage> : UnityEvent<TMessage> where TMessage : NetworkMessage<NetworkEvent>
{
    
}
