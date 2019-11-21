using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using UnityEngine;
using UnityEngine.Events;

public interface INetworkMessageEventsDatabase<TMessage> where TMessage : NetworkMessage<NetworkEvent>
{
    Type GetDatabaseMessageType();

    UnityEvent<TMessage> GetMessageCallback(NetworkEvent networkEvent);
}
