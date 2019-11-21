using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.Messaging.MessageHandling;
using UnityEngine;
using UnityEngine.Experimental.XR;

public class UnityNetworkMessageHandler : MonoSingleton<UnityNetworkMessageHandler>, INetworkMessageHandler<NetworkMessage<NetworkEvent>, NetworkEvent>
{
    public UnityStringMessageEventDatabase StringMessageEvents;
    public UnityImageMessageEventDatabase ImageMessageEvent;

    private Queue<KeyValuePair<Type, NetworkMessage<NetworkEvent>>> _messagesToHandleQueue;
    private bool _coRoutineRunning;
    void Start()
    {
        Debug.Log(FindObjectsOfType<UnityBaseMessageEventsDatabase<BaseNetworkMessage, BaseMessageCallbackWrapper<BaseNetworkMessage, BaseMessageCallback<BaseNetworkMessage>>, BaseMessageCallback<BaseNetworkMessage>>>().Length);
        
        _messagesToHandleQueue = new Queue<KeyValuePair<Type, NetworkMessage<NetworkEvent>>>();
        _coRoutineRunning = false;
    }
    
    public void MessageHandled(NetworkMessage<NetworkEvent> message, Type type)
    {
        _messagesToHandleQueue.Enqueue(new KeyValuePair<Type, NetworkMessage<NetworkEvent>>(type, message));
    }

    void Update()
    {
        if(!QueueHasMessages())
            return;

        if (!_coRoutineRunning)
            StartCoroutine(HandleMessages());
    }

    private bool QueueHasMessages()
    {
        return _messagesToHandleQueue.Count > 0;
    }

    private IEnumerator HandleMessages()
    {
        _coRoutineRunning = true;
        while (QueueHasMessages())
        {
            
            yield return new WaitForEndOfFrame();
        }
        _coRoutineRunning = false;
    }

}
