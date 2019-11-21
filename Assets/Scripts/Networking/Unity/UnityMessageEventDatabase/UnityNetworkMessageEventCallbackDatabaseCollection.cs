using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using UnityEngine;

public class UnityNetworkMessageEventCallbackDatabaseCollection<TDatabase, TMessage, TCallbackWrapper, TCallBack> : MemoryDatabase<Type, TDatabase, UnityNetworkMessageEventCallbackDatabaseCollection<TDatabase, TMessage, TCallbackWrapper, TCallBack>>
        where TDatabase : UnityNetworkMessageEventsDatabase<TMessage, TCallbackWrapper, TCallBack>
        where TMessage : NetworkMessage<NetworkEvent>
        where TCallBack : NetworkMessageCallback<TMessage>
        where TCallbackWrapper : NetworkMessageCallbackWrapper<TMessage, TCallBack>
{
    public void AddEventsDataBase(TDatabase database)
    {
        AddNewValue(database.GetDatabaseMessageType(), database);
    }

    public int Count()
    {
        return this.GetCount();
    }
}
