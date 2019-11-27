using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.Message;
using GameFrame.Networking.Messaging.MessageHandling;
using UnityEngine;
using UnityEngine.Experimental.XR;

public class UnityNetworkMessageHandler : MonoSingleton<UnityNetworkMessageHandler>, INetworkMessageHandler<NetworkMessage<NetworkEvent>, NetworkEvent>
{
    public int MaxMessagesPerFrame;

    public UnityEventOnlyMessageEventDatabase EventsOnlyMessageEvents;
    public UnityStringMessageEventDatabase StringMessageEvents;
    public UnityImageMessageEventDatabase ImageMessageEvents;

    private Queue<NetworkMessage<NetworkEvent>> _messagesToHandleQueue;
    private bool _coRoutineRunning;
    void Start()
    {
        Debug.Log(FindObjectsOfType<UnityBaseMessageEventsDatabase<BaseNetworkMessage, BaseMessageCallbackWrapper<BaseNetworkMessage, BaseMessageCallback<BaseNetworkMessage>>, BaseMessageCallback<BaseNetworkMessage>>>().Length);
        
        _messagesToHandleQueue = new Queue<NetworkMessage<NetworkEvent>>();
        _coRoutineRunning = false;
    }

    public void MessageHandled(NetworkMessage<NetworkEvent> message)
    {
        _messagesToHandleQueue.Enqueue(message);
    }

    void Update()
    {
        if(!QueueHasMessages() || _coRoutineRunning)
            return;

        StartCoroutine(HandleMessages());
    }

    private bool QueueHasMessages()
    {
        if(_messagesToHandleQueue == null)
            _messagesToHandleQueue = new Queue<NetworkMessage<NetworkEvent>>();
        return _messagesToHandleQueue.Count > 0;
    }

    private IEnumerator HandleMessages()
    {
        Debug.Log("Started handling messages");
        _coRoutineRunning = true;
        while (QueueHasMessages())
        {
            if (_messagesToHandleQueue.Count > MaxMessagesPerFrame)
            {
                HandleAmountOfMessages(MaxMessagesPerFrame);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                HandleAmountOfMessages(_messagesToHandleQueue.Count);
            }
            yield return new WaitForEndOfFrame();
        }
        _coRoutineRunning = false;
    }

    private void HandleAmountOfMessages(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var message = _messagesToHandleQueue.Dequeue();

            HandleSingleMessage(message);
        }
    }

    private void HandleSingleMessage(NetworkMessage<NetworkEvent> message)
    {
        switch (message)
        {
            case EventOnlyNetworkMessage eventsOnlyMessage:
                EventsOnlyMessageEvents.CallMessageCallback(eventsOnlyMessage);
                break;
            case StringNetworkMessage stringMessage:
                StringMessageEvents.CallMessageCallback(stringMessage);
                break;

            case ImageNetworkMessage imageMessage:
                ImageMessageEvents.CallMessageCallback(imageMessage);
                break;
        }
    }

}
